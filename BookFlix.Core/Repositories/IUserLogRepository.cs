using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IUserLogRepository
    {
        Task<UserLog> AddAsync(UserLog userLog);
        Task<IReadOnlyCollection<UserLog>> GetLogsByUserIdAsync(int userId);
    }
}
