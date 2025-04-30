using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies
{
    public class StandardRoomAddParticipantStrategy : IAddParticipantStrategy
    {
        public bool AddParticipant(Participant participant, Room room)
        {
            if (room.Participants.Count < Room.MAX_STANDARD_PARTICIPANTS)
            {
                room.AddParticipant(participant);
                return true;
            }

            return false;
        }
    }
}
