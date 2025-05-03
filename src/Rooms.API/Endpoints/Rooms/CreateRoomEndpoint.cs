using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.API.Application.Services.Interfaces;

namespace Rooms.API.Endpoints.Rooms
{
    public sealed class CreateRoomEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
            .Accepts<CreateRoomCommand>("application/json")
            .Produces<Response<CreateRoomResponse>>();

        private static async Task<IResult> HandleAsync(CreateRoomCommand command, IRoomService roomService)
            => Endpoint.CustomResponse(await roomService.CreateAsync(command).ConfigureAwait(false));
    }
}