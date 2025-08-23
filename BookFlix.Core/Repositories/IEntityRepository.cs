namespace BookFlix.Core.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<bool> IsExistById(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}
