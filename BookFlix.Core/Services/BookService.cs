using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;

namespace BookFlix.Core.Services
{
    public class BookService : IBookService
    {

        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
        }

        private async Task _ValidateISBNAsync(string? isbn, ValidationResult result)
        {
            // Check if ISBN used before
            var book = await _bookRepository.GetByISBNAsync(isbn);
            if (book is not null)
            {
                result.Errors.Add($"A book with ISBN {isbn} already exists.");
            }
        }

        public async Task<ValidationResult> ValidateCreateBookDtoAsync(CreateBookDto createBookDto)
        {
            var result = new ValidationResult();


            if (createBookDto.PublicationDate is not null && createBookDto.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
            }
            await _ValidateISBNAsync(createBookDto.ISBN, result);

            return result;
        }
    }
}
