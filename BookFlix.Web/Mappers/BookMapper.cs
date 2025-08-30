using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Web.Dtos.Author;
using BookFlix.Web.Dtos.Book;
using BookFlix.Web.Dtos.Genre;
using BookFlix.Web.Mapper_Interfaces;


namespace BookFlix.Web.Mappers
{
    public class BookMapper : IBookMapper
    {

        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        public BookMapper(IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;

        }

        private async Task<List<Author>> GetAuthors(List<int> authorIds)
        {
            var authors = new List<Author>();
            foreach (var authorId in authorIds)
            {
                var author = await _authorRepository.GetByIdAsync(authorId);
                if (author is null)
                {
                    throw new InvalidOperationException($"Author with ID {authorId} not found.");
                }

                authors.Add(author);
            }
            return authors;
        }

        private async Task<List<Genre>> GetGenres(List<int> genreIds)
        {
            var genres = new List<Genre>();

            foreach (var genreId in genreIds)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre is null)
                {
                    throw new InvalidOperationException($"genreId with ID {genreId} not found.");
                }
                genres.Add(genre);
            }
            return genres;
        }

        public BookDto ToBookDto(Book book)
        {

            BookDto bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ISBN = book.ISBN,
                CoverImageUrl = book.CoverImageUrl,
                PublicationDate = book.PublicationDate,
                Publisher = book.Publisher,
                PageCount = book.PageCount,
                AverageRating = book.AverageRating,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
                Authors = book.Authors?.Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList() ?? new List<AuthorDto>(),
                Genres = book.Genres?.Select(g => new GenreDto
                {
                    Id = g.Id,
                    Name = g.Name
                }).ToList() ?? new List<GenreDto>()
            };
            return bookDto;
        }

        public async Task<Book> ToBook(BookCreateDto bookCreateDto)
        {
            var authors = await GetAuthors(bookCreateDto.AuthorIds);
            var genres = await GetGenres(bookCreateDto.GenreIds);

            Book book = new Book
            {
                Title = bookCreateDto.Title,
                Description = bookCreateDto.Description,
                ISBN = bookCreateDto.ISBN,
                CoverImageUrl = bookCreateDto.CoverImageUrl,
                PublicationDate = bookCreateDto.PublicationDate,
                Publisher = bookCreateDto.Publisher,
                PageCount = bookCreateDto.PageCount,
                IsAvailable = bookCreateDto.IsAvailable,
                FileLocation = bookCreateDto.FileLocation,
                Authors = authors,
                Genres = genres
            };
            return book;
        }

        public async Task<Book> ToBook(BookUpdateDto bookUpdateDto)
        {
            var authors = await GetAuthors(bookUpdateDto.AuthorIds);
            var genres = await GetGenres(bookUpdateDto.GenreIds);

            Book book = new Book
            {
                Id = bookUpdateDto.Id,
                Title = bookUpdateDto.Title,
                Description = bookUpdateDto.Description,
                ISBN = bookUpdateDto.ISBN,
                CoverImageUrl = bookUpdateDto.CoverImageUrl,
                PublicationDate = bookUpdateDto.PublicationDate,
                Publisher = bookUpdateDto.Publisher,
                PageCount = bookUpdateDto.PageCount,
                IsAvailable = bookUpdateDto.IsAvailable,
                FileLocation = bookUpdateDto.FileLocation,
                Authors = authors,
                Genres = genres
            };
            return book;
        }
    }
}
