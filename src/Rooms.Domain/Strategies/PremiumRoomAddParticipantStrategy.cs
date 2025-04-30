using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies
{
    public class PremiumRoomAddParticipantStrategy : IAddParticipantStrategy
    {
        public bool AddParticipant(Participant participant, Room room)
        {
            if (room.Participants.Count < Room.MAX_PREMIUM_PARTICIPANTS)
            {
                room.AddParticipant(participant);
                return true;
            }

            return false;
        }
    }
}
