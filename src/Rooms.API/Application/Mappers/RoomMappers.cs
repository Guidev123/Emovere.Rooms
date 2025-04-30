using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.Domain.Entities;

namespace Rooms.API.Application.Mappers
{
    public static class RoomMappers
    {
        public static Room MapToEntity(this CreateRoomCommand command) 
            => new(command.HostId, command.Name, command.Details, command.StartDate, command.EndDate);
    }
}
