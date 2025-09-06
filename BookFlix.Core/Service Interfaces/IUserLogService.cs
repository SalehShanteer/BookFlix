using BookFlix.Core.Models;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IUserLogService
    {
        Task<IReadOnlyCollection<UserLog>> GetLogsByUserIdAsync(int userId);
    }

}
