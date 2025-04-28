using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies.Contexts
{
    public interface IAddParticipantContext
    {
        IAddParticipantStrategy SetStrategy(IAddParticipantStrategy strategy);
        bool AddParticipant(Participant participant, Room room);
    }
}
