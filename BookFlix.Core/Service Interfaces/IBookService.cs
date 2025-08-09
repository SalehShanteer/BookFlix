using BookFlix.Core.Dtos.Book;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IBookService
    {
        Task<ValidationResult> ValidateCreateBookDtoAsync(BookInputDto? createBookDto);

    }
}
