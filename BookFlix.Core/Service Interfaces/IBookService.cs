using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Services;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IBookService
    {
        Task<ValidationResult> ValidateCreateBookDtoAsync(CreateBookDto createBookDto);

    }
}
