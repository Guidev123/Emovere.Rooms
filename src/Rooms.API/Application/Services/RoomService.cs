using Emovere.SharedKernel.Abstractions.Mediator;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Emovere.WebApi.Services;
using Rooms.API.Application.Commands.Rooms.Create;
using Rooms.API.Application.Services.Interfaces;
using Rooms.Domain.Enums;

namespace Rooms.API.Application.Services
{
    public sealed class RoomService(IMediatorHandler mediatorHandler,
                                    INotificator notificator,
                                    IAspNetUserService aspNetUser) : IRoomService
    {
        public async Task<Response<CreateRoomResponse>> CreateAsync(CreateRoomCommand command)
        {
            var userId = aspNetUser.GetUserId();
            if (userId == Guid.Empty)
            {
                notificator.HandleNotification(new(EReportMessages.USER_NOT_FOUND.GetEnumDescription()));
                return Response<CreateRoomResponse>.Failure(Notifications, code: StatusCode.NOT_FOUND_STATUS_CODE);
            }

            command.SetHostId(userId);
            return await mediatorHandler.SendCommand(command).ConfigureAwait(false);
        }

        private List<string> Notifications => [.. notificator.GetNotifications().Select(x => x.Message)];
    }
}