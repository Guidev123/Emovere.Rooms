using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.Create;

namespace Rooms.API.Application.Services.Interfaces
{
    public interface IRoomService
    {
        Task<Response<CreateRoomResponse>> CreateAsync(CreateRoomCommand command);
    }
}