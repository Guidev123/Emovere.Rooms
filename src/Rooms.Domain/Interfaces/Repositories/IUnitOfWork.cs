namespace Rooms.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task<bool> RollbackAsync();
        Task<bool> SaveChangesAsync();
    }
}
