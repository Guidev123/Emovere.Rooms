using Rooms.Domain.Entities;

namespace Rooms.Domain.Strategies.Contexts
{
    public class AddParticipantContext : IAddParticipantContext
    {
        private IAddParticipantStrategy _strategy = null!;
        public bool AddParticipant(Participant participant, Room room)
            => _strategy.AddParticipant(participant, room);

        public IAddParticipantStrategy SetStrategy(IAddParticipantStrategy strategy)
        {
            _strategy = strategy;
            return _strategy;
        }
    }
}
