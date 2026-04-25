using BookFlix.Core.Models;
using BookFlix.Web.Dtos.Book;

namespace BookFlix.Web.Mapper_Interfaces
{
    public interface IBookMapper
    {
        BookDto ToBookDto(Book book);
        IList<BookDto> ToBookDtos(IList<Book> books);
        Task<Book> ToBook(BookCreateDto bookCreateDto);
        Task<Book> ToBook(BookUpdateDto bookUpdateDto);
    }
}
