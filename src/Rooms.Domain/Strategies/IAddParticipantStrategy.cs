using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies
{
    public interface IAddParticipantStrategy
    {
        bool AddParticipant(Participant participant, Room room);
    }
}
