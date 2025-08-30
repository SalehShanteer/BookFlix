using BookFlix.Core.Models;
using BookFlix.Web.Dtos.Book;

namespace BookFlix.Web.Mapper_Interfaces
{
    public interface IBookMapper
    {
        BookDto ToBookDto(Book book);
        Task<Book> ToBook(BookCreateDto bookCreateDto);
        Task<Book> ToBook(BookUpdateDto bookUpdateDto);

    }
}
