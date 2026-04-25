using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IGenreRepository
    {
        Task<Genre> GetByIDAsync(int id);
        Task<IReadOnlyCollection<Genre>> GetAllAsync();
    }
}
