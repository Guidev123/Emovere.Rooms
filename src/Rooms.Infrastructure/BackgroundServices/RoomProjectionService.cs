using Emovere.Communication.IntegrationEvents;
using Emovere.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Infrastructure.Data.Contexts;

namespace Rooms.Infrastructure.BackgroundServices
{
    public sealed class RoomProjectionService(IMessageBus messageBus,
                                              IServiceProvider serviceProvider)
                                            : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private async Task CreateRoomAsync(CreatedRoomIntegrationEvent @event)
        {
            using var scope = serviceProvider.CreateScope();
            var roomReadModel = scope.ServiceProvider.GetRequiredService<RoomsReadDbContext>();

            roomReadModel.Rooms.Add(MapToReadModel(@event));
            await roomReadModel.SaveChangesAsync();
        }

        private void OnConnect(object s, EventArgs e) => SetSubscribers();

        private void SetSubscribers()
        {
            messageBus.SubscribeAsync<CreatedRoomIntegrationEvent>("CreatedRoom", CreateRoomAsync);
            messageBus.AdvancedBus.Connected += OnConnect!;
        }

        private static Room MapToReadModel(CreatedRoomIntegrationEvent @event)
            => Room.Reconstruct(@event.RoomId, @event.HostId, @event.Name,
                @event.Details, @event.MaxParticipantsNumber,
                (ERoomPlan)@event.Plan,
                (ERoomStatus)@event.Status,
                @event.StartDate, @event.EndDate,
                @event.Price, @event.ParticipantsQuantity);
    }
}
