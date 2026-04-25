using BookFlix.Core.Models;
using BookFlix.Core.Repositories;

namespace BookFlix.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        public Task<Review> AddAsync(Review entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Review>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetByIDAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetByIDForUpdateAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistByIDAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Review> UpdateAsync(Review entity)
        {
            throw new NotImplementedException();
        }
    }
}
