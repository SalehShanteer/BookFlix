using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role is null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyCollection<Role>> GetAllAsync() 
            => await _context.Roles
            .AsNoTracking()
            .ToListAsync();

        public async Task<Role> GetByIDAsync(Guid id)
            => await _context.Roles.FindAsync(id);

        public async Task<Role> GetByIDForUpdateAsync(Guid id)
            => await _context.Roles.FindAsync(id);
        public Task<bool> IsExistByIDAsync(Guid id) => _context.Roles.AnyAsync(r => r.ID == id);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
