using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;
using BookFlix.Web.Dtos.Book;

namespace BookFlix.Web.Mapper_Interfaces
{
    public interface IBookMapper
    {
        BookDto ToBookDto(Book book);
        Task<(Book Book, ValidationResult Result)> ToBook(BookCreateDto bookCreateDto);
        Task<(Book Book, ValidationResult Result)> ToBook(BookUpdateDto bookUpdateDto);
    }
}
