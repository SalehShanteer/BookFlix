using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityRepository<Book>
    {
        Task<IReadOnlyCollection<Book>> GetByAuthorIdAsync(int authorId);
        Task<Book?> GetByISBNAsync(string? isbn);

    }

}
