using Rooms.Domain.Enums;

namespace Rooms.Domain.Strategies.Factories
{
    public class AddParticipantStrategyFactory : IAddParticipantStrategyFactory
    {
        private IAddParticipantStrategy _roomAddParticipantStrategy = null!;
        public IAddParticipantStrategy GetStrategy(ERoomPlan plan)
        {
            switch (plan)
            {
                case ERoomPlan.Standard:
                    _roomAddParticipantStrategy = new StandardRoomAddParticipantStrategy();
                    return _roomAddParticipantStrategy;
                case ERoomPlan.Premium:
                    _roomAddParticipantStrategy = new PremiumRoomAddParticipantStrategy();
                    return _roomAddParticipantStrategy;
                case ERoomPlan.Vip:
                    _roomAddParticipantStrategy = new VipRoomAddParticipantStrategy();
                    return _roomAddParticipantStrategy;
                case ERoomPlan.Exclusive:
                    _roomAddParticipantStrategy = new ExclusiveRoomAddParticipantStrategy();
                    return _roomAddParticipantStrategy;
                default:
                    return _roomAddParticipantStrategy;
            }
        }
    }
}
