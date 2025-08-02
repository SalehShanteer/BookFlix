using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityRepository<Book>
    {
        Task<List<Book>> GetByAuthorIdAsync(int authorId);
    }

}
