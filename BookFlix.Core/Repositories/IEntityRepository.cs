namespace BookFlix.Core.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<bool> IsExistById(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
