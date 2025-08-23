using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Repositories
{
    public interface ITransactionRepository
    {
        Task<ITransaction> BeginTransactionAsync();
    }

}
