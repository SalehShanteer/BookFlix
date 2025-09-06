using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IUserService
    {
        Task<(ValidationResult Result, User? User)> AddUserAsync(User user);
        Task<(ValidationResult Result, User? User)> AddUserAsAdmin(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<(ValidationResult Result, User? User)> UpdateUserPasswordAsync(User user);
        Task<(ValidationResult Result, User? User)> UpdateUserUsernameAsync(User user);
        Task<(ValidationResult Result, User? User)> UpdateUserEmailAsync(User user);
        Task<IReadOnlyCollection<User>> GetAllUsersAsync();
    }

}
