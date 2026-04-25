using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IBookService
    {
        Task<Result<Book>> AddBookAsync(Book book);
        Task<IReadOnlyCollection<Book>> GetAllBooksAsync();
        Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(Guid authorId);
        Task<Book> GetBookByIdAsync(Guid id);
        Task<Book> GetBookByIdForUpdateAsync(Guid id);
        Task<Result<Book>> UpdateBookAsync(Book book);
        Task<Result> DeleteBookAsync(Guid id);
        Task<Book> GetBookByIsbnAsync(string isbn);
        //Task<bool> IsBookExistAsync(int id);
        //Task<bool> IsBookExistAsync(string isbn);
    }
}
