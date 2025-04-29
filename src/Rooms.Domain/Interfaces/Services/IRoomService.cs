using Rooms.Domain.Entities;

namespace Rooms.Domain.Interfaces.Services
{
    public interface IRoomService
    {
        Task<bool> AddParticipantAsync(Participant participant, Room room);
        Task<bool> RemoveParticipantAsync(Participant participant, Room room);
    }
}
