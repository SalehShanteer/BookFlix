using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;

namespace BookFlix.Core.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IUserLogRepository _userLogRepository;
        public UserLogService(IUserLogRepository userLogRepository)
        {
            _userLogRepository = userLogRepository;
        }

        public Task<IReadOnlyCollection<UserLog>> GetLogsByUserIdAsync(int userId)
            => _userLogRepository.GetLogsByUserIdAsync(userId);
    }
}
