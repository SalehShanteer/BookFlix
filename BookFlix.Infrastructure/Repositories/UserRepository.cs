using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return true;
        }

        public async Task<IReadOnlyCollection<User>> GetAllAsync()
            => await _context.Users.AsNoTracking().ToListAsync();

        public async Task<User> GetByEmailAsync(string email)
            => await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> GetByIDAsync(Guid id)
            => await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.ID == id);

        public async Task<User> GetByIDForUpdateAsync(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<User> GetByIDWithRelationsAsync(Guid id)
            => await _context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .Include(u => u.RefreshTokens)
                .Include(u => u.Reviews)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.ID == id);

        public async Task<bool> IsEmailExistAsync(string email)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);

        public async Task<bool> IsExistByIDAsync(Guid id)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.ID == id);

        public async Task<bool> IsUsernameExistAsync(string username)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Username == username);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}