using BookFlix.Core.Models;
using BookFlix.Core.Repositories;

namespace BookFlix.Infrastructure.Repositories
{

    public class BookRepository : IBookRepository
    {
        public Task<Book> AddAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> GetByAuthorIdAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetByIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> UpdateAsync(Book entity)
        {
            throw new NotImplementedException();
        }

        Task<bool> IEntityRepository<Book>.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
