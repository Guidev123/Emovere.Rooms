using Emovere.SharedKernel.DomainObjects;
using Rooms.Domain.Interfaces.Repositories;

namespace Rooms.Infrastructure.Data.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public Task BeginTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public Task PublishDomainEventsAsync<TEntity>(TEntity entity) where TEntity : Entity
        {
            throw new NotImplementedException();
        }

        public Task<bool> RollbackAsync()
        {
            throw new NotImplementedException();
        }
    }
}
