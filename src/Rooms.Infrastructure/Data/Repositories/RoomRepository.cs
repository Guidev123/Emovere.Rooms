using Rooms.Domain.Interfaces.Repositories;
using Rooms.Infrastructure.Data.Contexts;

namespace Rooms.Infrastructure.Data.Repositories
{
    public sealed class RoomRepository(RoomsWriteDbContext writeContext,
                                       RoomsReadDbContext readContext)
                                     : IRoomRepository
    {
        public void Dispose()
        {
            writeContext.Dispose();
            readContext.Dispose();
        }
    }
}
