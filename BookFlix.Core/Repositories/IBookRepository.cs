using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityRepository<Book>, ITransactionRepository
    {
        Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(Guid authorId);
        Task<string> GetFileLocationAsync(Guid id);
        Task<Book> GetByISBNAsync(string isbn);
        Task<Book> GetByIdForUpdateAsync(Guid id);
        Task<bool> IsExistByIsbnAsync(string isbn);
        Task<bool> IsExistByIsbnAsync(Guid id, string isbn);
        Task<bool> UpdateFileLocationAsync(Guid id, string fileLocation);
    }

}
