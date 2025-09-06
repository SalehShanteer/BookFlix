using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IAuthService
    {
        Task<(User? User, ValidationResult Result)> LoginAsync(string email, string password, string ipAddress);
    }
}
