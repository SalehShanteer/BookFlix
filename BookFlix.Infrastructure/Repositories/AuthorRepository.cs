using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;

namespace BookFlix.Infrastructure.Repositories
{

    public class AuthorRepository : IAuthorRepository
    {

        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Author> AddAsync(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Author?> GetByIdAsync(int id) => await _context.Authors.FindAsync(id);

        public Task UpdateAsync(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
