using Emovere.SharedKernel.Notifications;
using Rooms.Domain.Entities;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Strategies.Factories;

namespace Rooms.Domain.DomainServices
{
    public sealed class RoomDomainService(IAddParticipantStrategyFactory strategyFactory,
                                           INotificator notificator)
                                         : IRoomDomainService
    {
        public void AddParticipant(Participant participant, Room room)
        {
            var participantAddedSuccessfully = strategyFactory
                .GetStrategy(room.Plan)
                .AddParticipant(participant, room);

            if(!participantAddedSuccessfully)
                notificator.HandleNotification(new("Participant cannot be added to the room."));
        }

        public void RemoveParticipant(Participant participant, Room room) 
            => room.RemoveParticipant(participant);
    }
}
