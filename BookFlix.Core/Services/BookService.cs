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
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task ValidateISBNAsync(string? isbn, ValidationResult result)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                result.Errors.Add("ISBN cannot be null or empty.");
                _logger.LogWarning("ISBN validation failed: ISBN is null or empty.");
                return;
            }

            // Check if ISBN used before
            bool isExist = await _bookRepository.IsExistByIsbnAsync(isbn);
            if (isExist)
            {
                result.Errors.Add($"A book with ISBN {isbn} already exists.");
                _logger.LogWarning("ISBN validation failed: A book with ISBN {ISBN} already exists.", isbn);
            }
        }

        private async Task<ValidationResult> ValidatBookAsync(Book book)
        {
            var result = new ValidationResult();

            if (book.PublicationDate is not null && book.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
                _logger.LogWarning("Publication date validation failed: Date {PublicationDate} is in the future.", book.PublicationDate);
            }

            await ValidateISBNAsync(book.ISBN, result);

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

        public async Task<(ValidationResult result, Book? book)> UpdateBookAsync(Book book)
        {
            var result = await ValidatBookAsync(book);

            if (!result.IsValid)
            {
                _logger.LogError("Book update failed for ID {BookId} with {ErrorCount} validation errors: {@Errors}",
                    book.Id, result.Errors.Count, result.Errors);

                return (result, null);
            }

            book.UpdatedAt = DateTime.UtcNow;
            var updatedBook = await _bookRepository.UpdateAsync(book);
            return (result, updatedBook);
        }


        public Task<bool> UpdateBookFileLocationAsync(int id, string fileLocation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetBookByIsbnAsync(string isbn)
        {
            throw new NotImplementedException();
        }
    }
}