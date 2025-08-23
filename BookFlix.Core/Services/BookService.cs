using BookFlix.Core.Dtos.Book;
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
            bool isExist = await _bookRepository.IsExistByISBNAsync(isbn);
            if (isExist)
            {
                result.Errors.Add($"A book with ISBN {isbn} already exists.");
                _logger.LogWarning("ISBN validation failed: A book with ISBN {ISBN} already exists.", isbn);
            }
            else
            {
                _logger.LogInformation("ISBN {ISBN} is valid and not previously used.", isbn);
            }
        }

        public async Task<ValidationResult> ValidateCreateBookDtoAsync(BookInputDto? createBookDto)
        {
            var result = new ValidationResult();
            _logger.LogInformation("Starting validation for BookInputDto with ISBN {ISBN}.", createBookDto?.ISBN);

            if (createBookDto is null)
            {
                result.Errors.Add("Book input cannot be null.");
                _logger.LogError("BookInputDto validation failed: Input is null.");
                return result;
            }

            if (createBookDto.PublicationDate is not null && createBookDto.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
                _logger.LogWarning("Publication date validation failed: Date {PublicationDate} is in the future.", createBookDto.PublicationDate);
            }

            await ValidateISBNAsync(createBookDto.ISBN, result);

            if (result.Errors.Any())
            {
                _logger.LogError("BookInputDto validation failed with {ErrorCount} errors.", result.Errors.Count);
            }
            else
            {
                _logger.LogInformation("BookInputDto validation succeeded for ISBN {ISBN}.", createBookDto.ISBN);
            }

            return result;
        }
    }
}