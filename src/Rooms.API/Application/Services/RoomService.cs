using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.API.Application.Services.Interfaces;

namespace Rooms.API.Application.Services
{
    public sealed class RoomService(IMediatorHandler mediatorHandler) : IRoomService
    {
        public async Task<Response<CreateRoomResponse>> CreateAsync(CreateRoomCommand command)
            => await mediatorHandler.SendCommand(command).ConfigureAwait(false);
    }
}