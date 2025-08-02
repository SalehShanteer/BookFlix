using BookFlix.Core.Models;
using BookFlix.Core.Repositories;

namespace BookFlix.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
