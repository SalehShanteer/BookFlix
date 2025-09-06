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
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyCollection<User>> GetAllAsync()
            => await _context.Users.AsNoTracking().ToListAsync();

        public async Task<User?> GetByEmailAsync(string email)
            => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);


        public async Task<User?> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<bool> IsEmailExist(string email)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);

        public async Task<bool> IsExistById(int id)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Id == id);

        public async Task<bool> IsUsernameExist(string username)
            => await _context.Users.AsNoTracking().AnyAsync(u => u.Username == username);


        public async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
