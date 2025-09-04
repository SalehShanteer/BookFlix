using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityRepository<Book>, ITransactionRepository
    {
        Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId);
        Task<(string? FileLocation, bool IsBookExist)> GetFileLocationAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<Book?> GetByIdForUpdateAsync(int id);
        Task<bool> IsExistByIsbnAsync(string isbn);
        Task<bool> IsExistByIsbnAsync(int id, string isbn);
        Task<bool> UpdateFileLocationAsync(int id, string fileLocation);
    }

}
