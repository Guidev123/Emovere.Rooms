using Emovere.SharedKernel.Responses;
using Rooms.API.Endpoints.Rooms;

namespace Rooms.API.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            endpoints.MapGroup("api/v1/rooms")
                .WithTags("Rooms")
                .MapEndpoint<CreateRoomEndpoint>();
        }

        public static IResult CustomResponse<T>(Response<T> response)
        {
            return response.Code switch
            {
                200 => Results.Ok(response),
                201 => Results.Created(string.Empty, response),
                204 => Results.NoContent(),
                400 => Results.BadRequest(response),
                404 => Results.NotFound(response),
                _ => Results.InternalServerError(response)
            };
        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
            where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
