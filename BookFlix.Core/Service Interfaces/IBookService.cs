using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IBookService
    {
        Task<(ValidationResult result, Book? book)> AddBookAsync(Book book);
        Task<IReadOnlyCollection<Book>> GetAllBooksAsync();
        Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(int authorId);
        Task<Book?> GetBookByIdAsync(int id);
        Task<(ValidationResult result, Book? book)> UpdateBookAsync(Book book);
        Task<bool> UpdateBookFileLocationAsync(int id, string fileLocation);
        Task<bool> DeleteBookAsync(int id);
        Task<Book?> GetBookByIsbnAsync(string isbn);
        //Task<bool> IsBookExistAsync(int id);
        //Task<bool> IsBookExistAsync(string isbn);
    }
}
