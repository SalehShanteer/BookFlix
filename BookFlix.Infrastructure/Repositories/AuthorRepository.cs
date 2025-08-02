using BookFlix.Core.Models;
using BookFlix.Core.Repositories;

namespace BookFlix.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public Task<Author> AddAsync(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Author> UpdateAsync(Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
