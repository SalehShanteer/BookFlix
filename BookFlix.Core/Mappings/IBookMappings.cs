using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Models;

namespace BookFlix.Core.Mappings
{
    public interface IBookMappings
    {
        Task<Book> ToBook(BookInputDto createBookDto);

        BookDto ToBookDto(Book book);
    }
}
