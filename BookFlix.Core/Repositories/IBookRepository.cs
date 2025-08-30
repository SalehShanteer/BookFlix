using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityRepository<Book>, ITransactionRepository
    {
        Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId);
        Task<(string? FileLocation, bool IsExist)> GetFileLocationAsync(int id);
        Task<Book?> GetByISBNAsync(string isbn);
        Task<bool> IsExistByIsbnAsync(string isbn);
        Task<bool> UpdateFileLocationAsync(int id, string fileLocation);
    }

}
