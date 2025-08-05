using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Repositories;

namespace BookFlix.Core.Services
{
    public class BookService
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

        private async Task _ValidateAuthorsAsync(List<int> authorIds, ValidationResult result)
        {
            var notFoundAuthorIds = new List<int>();
            foreach (var id in authorIds)
            {
                var author = await _authorRepository.GetByIdAsync(id);
                if (author is null)
                {
                    notFoundAuthorIds.Add(id);
                }
            }

            if (notFoundAuthorIds.Count > 0)
            {
                result.Errors.Add($"Authors with IDs {string.Join(", ", notFoundAuthorIds)} do not exist.");
            }
        }

        private async Task _ValidateGenresAsync(List<int> genreIds, ValidationResult result)
        {
            var notFoundgenreIds = new List<int>();
            foreach (var id in genreIds)
            {
                var author = await _genreRepository.GetByIdAsync(id);
                if (author is null)
                {
                    notFoundgenreIds.Add(id);
                }
            }

            if (notFoundgenreIds.Count > 0)
            {
                result.Errors.Add($"Genres with IDs {string.Join(", ", notFoundgenreIds)} do not exist.");
            }
        }

        public async Task<ValidationResult> ValidateCreateBookDtoAsync(CreateBookDto createBookDto)
        {
            var result = new ValidationResult();

            //await _ValidateISBN(createBookDto.ISBN, result);

            if (createBookDto.PublicationDate is not null && createBookDto.PublicationDate > DateTime.Now)
            {
                result.Errors.Add("The publication date should not be in the future");
            }
            //await _ValidateAuthors(createBookDto.AuthorIds, result);

            await Task.WhenAll(_ValidateISBNAsync(createBookDto.ISBN, result),
                _ValidateAuthorsAsync(createBookDto.AuthorIds, result),
                _ValidateGenresAsync(createBookDto.GenreIds, result));

            return result;
        }
    }
}
