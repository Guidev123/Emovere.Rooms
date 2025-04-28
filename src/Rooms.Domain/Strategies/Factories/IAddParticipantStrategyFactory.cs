using Rooms.Domain.Enums;

namespace Rooms.Domain.Strategies.Factories
{
    public interface IAddParticipantStrategyFactory
    {
        IAddParticipantStrategy GetStrategy(ERoomPlan plan);
    }
}
