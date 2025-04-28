using Rooms.Domain.Entities;

namespace Rooms.Domain.Interfaces.Repositories
{
    public interface IRoomRepository : IDisposable
    {
        Task<(IEnumerable<Room> Rooms, int count)> GetAllByHostIdAsync(Guid hostId, int pageNumber, int pageSize);
        Task<Room?> GetByIdAsync(Guid id);
        void Update(Room room);
        void Create(Room room);
        void Delete(Room room);
    }
}
