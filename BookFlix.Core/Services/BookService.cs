using BookFlix.Core.Helpers;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IFileService _fileService;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, IFileService fileService, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _fileService = fileService;
            _logger = logger;
        }

        private async Task ValidateISBNAsync(Book book, ValidationResult result, bool isNew)
        {
            var isbn = book.ISBN;
            if (string.IsNullOrEmpty(isbn))
            {
                _logger.LogErrorForValidation("ISBN cannot be null or empty.", result);
                return;
            }

            // Check if ISBN used before
            bool isExist = (isNew == true) ? await _bookRepository.IsExistByIsbnAsync(isbn) : await _bookRepository.IsExistByIsbnAsync(book.Id, isbn);
            if (isExist)
            {
                _logger.LogErrorForValidation($"A book with ISBN {isbn} already exists.", result);
            }
        }

        private async Task<ValidationResult> ValidatBookAsync(Book book, bool isNew = true)
        {
            var result = new ValidationResult();

            if (book.PublicationDate is not null && book.PublicationDate > DateTime.Now)
            {
                _logger.LogErrorForValidation("The publication date should not be in the future", result);
            }

            await ValidateISBNAsync(book, result, isNew);

            if (result.Errors.Any())
            {
                _logger.LogErrorForValidation($"BookInputDto validation failed with {result.Errors.Count} errors.", result);
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
                _logger.LogErrorForValidation($"Failed to add book with ISBN {book.ISBN}. Validation errors: {@result.Errors}", result);
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
                _logger.LogErrorForValidation($"Book update failed for ID = {updatedBook.Id} with {result.Errors.Count} validation errors: {@result.Errors}",
                    result);
                result.StatusCode = enStatusCode.BadRequest;
                return (result, null);
            }

            var existingBook = await _bookRepository.GetByIdForUpdateAsync(updatedBook.Id);
            if (existingBook is null)
            {
                _logger.LogErrorForValidation($"Book with ID {updatedBook.Id} not found.", result);
                result.StatusCode = enStatusCode.NotFound;
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
                    _logger.LogErrorForValidation($"There is no book with id = {id}.", result);
                }

                await transaction.CommitAsync();
                return result;
            }
            catch (IOException ex)
            {
                await transaction.RollbackAsync();

                _logger.LogExceptionErrorForValidation(ex, $"IO error deleting file for book ID {id}", result);
                result.StatusCode = enStatusCode.InternalServerError;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogExceptionErrorForValidation(ex, $"Unexpected error deleting book ID {id}", result);
                result.StatusCode = enStatusCode.InternalServerError;
                return result;
            }
        }

        public async Task<Book?> GetBookByIsbnAsync(string isbn) => await _bookRepository.GetByISBNAsync(isbn);
    }
}