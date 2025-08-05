using BookFlix.Core.Dtos.Author;
using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Dtos.Genre;
using BookFlix.Core.Models;

namespace BookFlix.Core.Mappings
{
    public static class BookMappings
    {
        public static BookDto ToBookDto(Book book)
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


        public static Book ToBook(CreateBookDto createBookDto)
        {
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
                Authors = createBookDto.AuthorIds.Select(id => new Author { Id = id }).ToList(),
                Genres = createBookDto.GenreIds.Select(id => new Genre { Id = id }).ToList()
            };
            return book;
        }
    }
}