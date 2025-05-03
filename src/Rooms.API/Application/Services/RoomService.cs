using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.API.Application.Services.Interfaces;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Services;

namespace Rooms.API.Application.Services
{
    public sealed class RoomService(IMediatorHandler mediatorHandler,
                                    INotificator notificator,
                                    IAspNetUser aspNetUser) : IRoomService
    {
        public async Task<Response<CreateRoomResponse>> CreateAsync(CreateRoomCommand command)
        {
            var userId = await aspNetUser.GetUserIdAsync().ConfigureAwait(false);
            if (userId == Guid.Empty || !userId.HasValue)
            {
                notificator.HandleNotification(new(EReportMessages.USER_NOT_FOUND.GetEnumDescription()));
                return Response<CreateRoomResponse>.Failure(Notifications, code: 404);
            }

            command.SetHostId(userId.Value);
            return await mediatorHandler.SendCommand(command).ConfigureAwait(false);
        }

        private List<string> Notifications => [.. notificator.GetNotifications().Select(x => x.Message)];
    }
}