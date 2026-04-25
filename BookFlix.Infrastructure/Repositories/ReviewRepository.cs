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

        public Task<Review> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Review> UpdateAsync(Review entity)
        {
            throw new NotImplementedException();
        }
    }
}
