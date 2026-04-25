using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IAuthService
    {
        Task<Result<(string AccessToken, string RefreshToken)>> LoginAsync(string email, string password, string ipAddress);
    }
}
