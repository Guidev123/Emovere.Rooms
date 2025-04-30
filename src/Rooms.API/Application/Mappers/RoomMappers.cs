using Emovere.Communication.IntegrationEvents;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.Domain.Entities;

namespace Rooms.API.Application.Mappers
{
    public static class RoomMappers
    {
        public static Room MapToEntity(this CreateRoomCommand command) 
            => new(command.HostId, command.Name, command.Details, command.StartDate, command.EndDate);

        public static CreatedRoomIntegrationEvent MapToIntegrationEvent(this Room room)
            => new(room.Id, room.HostId, room.RoomSpecification.Name, room.RoomSpecification.Description,
                room.RoomSpecification.MaxParticipantsNumber, (int)room.Plan, (int)room.Status,
                room.ParticipantsQuantity, room.Price, room.StartDate, room.EndDate);
    }
}
