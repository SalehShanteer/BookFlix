using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        AppDbContext _context;

        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<IReadOnlyCollection<Genre>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _context.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
