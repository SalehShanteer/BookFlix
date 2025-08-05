using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Genre>> GetAllAsync();

    }
}
