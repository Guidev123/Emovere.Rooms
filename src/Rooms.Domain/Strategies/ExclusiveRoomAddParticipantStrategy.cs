using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies
{
    public class ExclusiveRoomAddParticipantStrategy : IAddParticipantStrategy
    {
        public bool AddParticipant(Participant participant, Room room)
        {
            if (room.Participants.Count <= Room.MAX_EXCLUSIVE_PARTICIPANTS)
            {
                room.AddParticipant(participant);
                return true;
            }

            return false;
        }
    }
}
