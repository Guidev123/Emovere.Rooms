using Rooms.Domain.Entities;

namespace Rooms.Domain.Interfaces.Services
{
    public interface IRoomDomainService
    {
        void AddParticipant(Participant participant, Room room);
        void RemoveParticipant(Participant participant, Room room);
    }
}
