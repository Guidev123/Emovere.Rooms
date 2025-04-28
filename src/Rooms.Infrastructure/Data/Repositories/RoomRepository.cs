using Microsoft.EntityFrameworkCore;
using Rooms.Domain.Entities;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Infrastructure.Data.Contexts;

namespace Rooms.Infrastructure.Data.Repositories
{
    public sealed class RoomRepository(RoomsWriteDbContext writeContext,
                                       RoomsReadDbContext readContext)
                                     : IRoomRepository
    {
        public async Task<(IEnumerable<Room> Rooms, int count)> GetAllByHostIdAsync(Guid hostId, int pageNumber, int pageSize)
            => (await readContext.Rooms.AsNoTracking().Skip((pageNumber - 1)  * pageSize).Take(pageSize).ToListAsync(), 
                await readContext.Rooms.AsNoTracking().CountAsync());

        public async Task<Room?> GetByIdAsync(Guid id)
            => await readContext.Rooms.AsNoTrackingWithIdentityResolution().Include(r => r.Participants).FirstOrDefaultAsync(x => x.Id == id);

        public void Create(Room room)
            => writeContext.Rooms.Add(room);

        public void Update(Room room)
            => writeContext.Rooms.Update(room);

        public void Delete(Room room)
            => writeContext.Rooms.Remove(room);

        public void Dispose()
        {
            writeContext.Dispose();
            readContext.Dispose();
        }
    }
}
