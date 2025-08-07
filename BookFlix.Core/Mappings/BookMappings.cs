using BookFlix.Core.Dtos.Author;
using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Dtos.Genre;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;

namespace BookFlix.Core.Mappings
{
    public class BookMappings : IBookMappings
    {

        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;
        public BookMappings(IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;

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


        public async Task<Book> ToBook(BookInputDto createBookDto)
        {
            var authors = new List<Author>();
            var genres = new List<Genre>();

            foreach (var authorId in createBookDto.AuthorIds)
            {
                var author = await _authorRepository.GetByIdAsync(authorId);
                if (author is null)
                {
                    throw new InvalidOperationException($"Author with ID {authorId} not found.");
                }

                authors.Add(author);
            }

            foreach (var genreId in createBookDto.GenreIds)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre is null)
                {
                    throw new InvalidOperationException($"genreId with ID {genreId} not found.");
                }
                genres.Add(genre);
            }

            Book book = new Book
            {
                Title = createBookDto.Title,
                Description = createBookDto.Description,
                ISBN = createBookDto.ISBN,
                CoverImageUrl = createBookDto.CoverImageUrl,
                PublicationDate = createBookDto.PublicationDate,
                Publisher = createBookDto.Publisher,
                PageCount = createBookDto.PageCount,
                IsAvailable = createBookDto.IsAvailable,
                FileLocation = createBookDto.FileLocation,
                CreatedAt = DateTime.Now,
                Authors = authors,
                Genres = genres
            };
            return book;
        }
    }
}