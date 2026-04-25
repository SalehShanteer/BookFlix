using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetByIDAsync(Guid id) => await _context.Authors.FindAsync(id);

        public Task<Author> GetByIDForUpdateAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetByIDWithBooksAsync(Guid id)
            => await _context.Authors.AsNoTracking().Include(a => a.Books).FirstOrDefaultAsync(a => a.ID == id);

        public async Task<bool> IsExistByIDAsync(Guid id) => await _context.Authors.AsNoTracking().AnyAsync(a => a.ID == id);

        public Task<Author> UpdateAsync(Author entity)
        {
            throw new NotImplementedException();
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
