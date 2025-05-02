using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.Create;

namespace Rooms.API.Endpoints.Rooms
{
    public sealed class CreateRoomEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
            .Accepts<CreateRoomCommand>("application/json")
            .Produces<Response<CreateRoomResponse>>();

        private static async Task<IResult> HandleAsync(CreateRoomCommand command, IMediatorHandler mediatorHandler)
        {
            command.SetHostId(Guid.NewGuid());
            return Endpoint.CustomResponse(await mediatorHandler.SendCommand(command).ConfigureAwait(false));
        }
    }
}