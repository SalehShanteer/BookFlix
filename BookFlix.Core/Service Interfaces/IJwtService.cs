using BookFlix.Core.Models;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        RefreshToken GenerateRefreshToken(Guid userId);
        RefreshToken GenerateRefreshToken(Guid userId, DateTime expireDate);
        Task<bool> IsValidRefreshToken(string token);
    }

}
