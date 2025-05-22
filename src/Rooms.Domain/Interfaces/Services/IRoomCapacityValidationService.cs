using Rooms.Domain.Entities;

namespace Rooms.Domain.Interfaces.Services
{
    public interface IRoomCapacityValidationService
    {
        Task<bool> AddParticipantAsync(Participant participant, Room room);

        bool RemoveParticipant(Participant participant, Room room);
    }
}