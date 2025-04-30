using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Rooms.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublishEventsAsync(this IMediatorHandler mediatorHandler, DbContext context)
        {
            var domainEntities = context.ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Count != 0);

            var domainEvents = domainEntities.SelectMany(x => x.Entity.Events).ToList();

            domainEntities.ToList().ForEach(e => e.Entity.ClearEvents());

            var tasks = domainEvents.Select(async (domainEvents) =>
            {
                await mediatorHandler.PublishEventAsync(domainEvents);
            });

            await Task.WhenAll(tasks);
        }
    }
}
