using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IUserService
    {
        Task<(ValidationResult Result, User? User)> AddUserAsync(User user);
        Task<(ValidationResult Result, User? User)> AddUserAsAdminAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByRefreshToken(string token);
        Task<(ValidationResult Result, User? User)> UpdateUserPasswordAsync(User user, string oldPassword);
        Task<(ValidationResult Result, User? User)> UpdateUserUsernameAsync(User user);
        Task<(ValidationResult Result, User? User)> UpdateUserEmailAsync(User user);
        Task<(ValidationResult Result, string? AccessToken, string? RefreshToken)> UpdateUserRefreshToken(string refreshToken);
        Task<IReadOnlyCollection<User>> GetAllUsersAsync();
    }

}
