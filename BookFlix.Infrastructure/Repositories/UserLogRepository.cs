using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class UserLogRepository : IUserLogRepository
    {
        private readonly AppDbContext _context;
        public UserLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserLog> AddAsync(UserLog entity)
        {
            await _context.UserLogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IReadOnlyCollection<UserLog>> GetLogsByUserIdAsync(int userId)
            => await _context.UserLogs.AsNoTracking()
                                      .Where(ul => ul.UserId == userId)
                                      .OrderByDescending(ul => ul.Timestamp)
                                      .ToListAsync();
    }
}
