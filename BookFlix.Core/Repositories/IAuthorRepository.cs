using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IAuthorRepository : IEntityRepository<Author>
    {
        Task<Author?> GetByIdWithBooksAsync(int id);
    }
}
