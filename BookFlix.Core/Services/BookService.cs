using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IFileService _fileService;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IFileService fileService, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task ValidateISBNAsync(Book book, ValidationResult result, bool isNew)
        {
            var isbn = book.ISBN;
            if (string.IsNullOrEmpty(isbn))
            {
                result.Errors.Add("ISBN cannot be null or empty.");
                _logger.LogWarning("ISBN validation failed: ISBN is null or empty.");
                return;
            }

            // Check if ISBN used before
            bool isExist = (isNew == true) ? await _bookRepository.IsExistByIsbnAsync(isbn) : await _bookRepository.IsExistByIsbnAsync(book.Id, isbn);
            if (isExist)
            {
                result.Errors.Add($"A book with ISBN {isbn} already exists.");
                _logger.LogWarning("ISBN validation failed: A book with ISBN {ISBN} already exists.", isbn);
            }
        }

        private async Task<ValidationResult> ValidatBookAsync(Book book, bool isNew = true)
        {
            var result = new ValidationResult();

            if (book.PublicationDate is not null && book.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
                _logger.LogWarning("Publication date validation failed: Date {PublicationDate} is in the future.", book.PublicationDate);
            }

            await ValidateISBNAsync(book, result, isNew);

            if (result.Errors.Any())
            {
                _logger.LogError("BookInputDto validation failed with {ErrorCount} errors.", result.Errors.Count);
            }
            else
            {
                _logger.LogInformation("BookInputDto validation succeeded for ISBN {ISBN}.", book.ISBN);
            }

            return result;
        }

        private void UpdateBookProperties(Book existingBook, Book updatedBook)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.CoverImageUrl = updatedBook.CoverImageUrl;
            existingBook.PublicationDate = updatedBook.PublicationDate;
            existingBook.Publisher = updatedBook.Publisher;
            existingBook.PageCount = updatedBook.PageCount;
            existingBook.IsAvailable = updatedBook.IsAvailable;
            existingBook.UpdatedAt = DateTime.UtcNow;

            existingBook.Authors.Clear();
            foreach (var author in updatedBook.Authors)
                existingBook.Authors.Add(author);

            existingBook.Genres.Clear();
            foreach (var genre in updatedBook.Genres)
                existingBook.Genres.Add(genre);
        }

        public async Task<(ValidationResult result, Book? book)> AddBookAsync(Book book)
        {
            var result = await ValidatBookAsync(book);

            if (!result.IsValid)
            {
                _logger.LogWarning("Failed to add book with ISBN {ISBN}. Validation errors: {Errors}", book.ISBN, result.Errors);
                return (result, null);
            }

            var addedBook = await _bookRepository.AddAsync(book);
            return (result, addedBook);
        }

        public async Task<IReadOnlyCollection<Book>> GetAllBooksAsync() => await _bookRepository.GetAllAsync();

        public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId) => await _bookRepository.GetByAuthorIdAsync(authorId);

        public async Task<Book?> GetBookByIdAsync(int id) => await _bookRepository.GetByIdAsync(id);

        public async Task<Book?> GetBookByIdForUpdateAsync(int id) => await _bookRepository.GetByIdForUpdateAsync(id);

        public async Task<(ValidationResult result, Book? book)> UpdateBookAsync(Book updatedBook)
        {
            var result = new ValidationResult();

            result = await ValidatBookAsync(updatedBook, false);
            if (!result.IsValid)
            {
                _logger.LogError("Book update failed for ID = {BookId} with {ErrorCount} validation errors: {@Errors}",
                    updatedBook.Id, result.Errors.Count, result.Errors);
                return (result, null);
            }

            var existingBook = await _bookRepository.GetByIdForUpdateAsync(updatedBook.Id);
            if (existingBook is null)
            {
                result.Errors.Add($"Book with ID {updatedBook.Id} not found.");
                return (result, null);
            }

            UpdateBookProperties(existingBook, updatedBook);

            var saved = await _bookRepository.UpdateAsync(existingBook);

            return (result, saved);
        }


        public async Task<ValidationResult> DeleteBookAsync(int id)
        {
            ValidationResult result = new ValidationResult();

            using var transaction = await _bookRepository.BeginTransactionAsync();
            try
            {
                var bookResult = await _bookRepository.GetFileLocationAsync(id);
                var bookFileLocation = bookResult.FileLocation;

                if (bookFileLocation is not null)
                {
                    _fileService.DeleteBookFile(bookFileLocation, result);
                }

                if (!await _bookRepository.DeleteAsync(id))
                {
                    result.StatusCode = enStatusCode.NotFound;
                    result.Errors.Add($"There is no book with id = {id}.");
                }

                await transaction.CommitAsync();
                return result;
            }
            catch (IOException ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex, "IO error uploading file for book ID {BookId}", id);
                result.Errors.Add("Failed to save file due to a storage error.");
                result.StatusCode = enStatusCode.InternalServerError;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected error uploading file for book ID {BookId}", id);
                result.Errors.Add("An unexpected error occurred while uploading the file.");
                result.StatusCode = enStatusCode.InternalServerError;
                return result;
            }
        }

        public async Task<Book?> GetBookByIsbnAsync(string isbn) => await _bookRepository.GetByISBNAsync(isbn);


    }
}