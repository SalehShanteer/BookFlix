using BookFlix.Core.Models;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }

}
