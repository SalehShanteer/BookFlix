using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> AddUserAsync(User user);
        Task<Result<User>> AddUserAsAdminAsync(User user);
        Task<Result<User>> GetUserByIdAsync(Guid id);
        Task<User> GetUserByRefreshToken(string token);
        Task<Result> UpdateUserPasswordAsync(Guid userID, string oldPassword, string newPassword);
        Task<Result<User>> UpdateUserUsernameAsync(Guid id, string username);
        Task<Result<User>> UpdateUserEmailAsync(Guid id, string email);
        Task<Result<(string AccessToken, string RefreshToken)>> UpdateUserRefreshToken(string refreshToken);
        Task<IReadOnlyCollection<User>> GetAllUsersAsync();
    }
}
