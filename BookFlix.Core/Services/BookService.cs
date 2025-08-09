using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Services
{
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        private async Task _ValidateISBNAsync(string? isbn, ValidationResult result)
        {
            // Check if ISBN used before
            bool isExist = await _bookRepository.IsExistByISBNAsync(isbn);
            if (isExist)
            {
                result.Errors.Add($"A book with ISBN {isbn} already exists.");
            }
        }

        public async Task<ValidationResult> ValidateCreateBookDtoAsync(BookInputDto? createBookDto)
        {
            var result = new ValidationResult();

            if (createBookDto is null)
            {
                result.Errors.Add("Book input cannot be null.");
                return result;
            }

            if (createBookDto?.PublicationDate is not null && createBookDto.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
            }
            await _ValidateISBNAsync(createBookDto?.ISBN, result);

            return result;
        }
    }
}
