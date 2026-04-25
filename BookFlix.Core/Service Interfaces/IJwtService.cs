using BookFlix.Core.Models;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
        RefreshToken GenerateRefreshToken(Guid userID);
        RefreshToken GenerateRefreshToken(Guid userID, DateTime expireDate);
        Task<bool> IsValidRefreshToken(string token);
    }

}
