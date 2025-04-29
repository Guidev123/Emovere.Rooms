namespace Rooms.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task<bool> CommitAsync();
        Task<bool> RollbackAsync();
    }
}
