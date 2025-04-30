using Emovere.Infrastructure.EventSourcing;
using Emovere.SharedKernel.Abstractions.Mediator;
using Microsoft.EntityFrameworkCore.Storage;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Infrastructure.Data.Contexts;

namespace Rooms.Infrastructure.Data.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly RoomsWriteDbContext _writeDbContext;

        public UnitOfWork(IMediatorHandler mediatorHandler, RoomsWriteDbContext writeDbContext)
        {
            _mediatorHandler = mediatorHandler;
            _writeDbContext = writeDbContext;
        }

        public async Task BeginTransactionAsync()
            => _transaction = await _writeDbContext.Database.BeginTransactionAsync();

        public async Task<bool> CommitTransactionAsync()
        {
            if (_transaction is null) return false;

            await _transaction.CommitAsync();
            await PublishDomainEventsAsync();

            return true;
        }

        public async Task<bool> RollbackAsync()
        {
            if (_transaction is null) return false;

            await _transaction.RollbackAsync();
            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (_transaction is null)
                await PublishDomainEventsAsync();

            return await _writeDbContext.SaveChangesAsync() > 0;
        }

        private async Task PublishDomainEventsAsync()
            => await _mediatorHandler.PublishEventsAsync(_writeDbContext);

        public void Dispose()
        {
            _transaction?.Dispose();
            _writeDbContext?.Dispose();
        }
    }
}
