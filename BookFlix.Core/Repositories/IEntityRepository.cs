namespace BookFlix.Core.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        Task<T> GetByIDAsync(Guid id);
        Task<T> GetByIDForUpdateAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<bool> IsExistByIDAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task SaveChangesAsync();
    }
}
