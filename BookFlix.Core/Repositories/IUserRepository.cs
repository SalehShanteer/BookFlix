using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IUserRepository : IEntityRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsUsernameExist(string username);
    }
}
