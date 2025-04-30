using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Rooms.API.Application.Mappers;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Repositories;

namespace Rooms.API.Application.Commands.Rooms.Create
{
    public sealed class CreateRoomHandler(INotificator notificator,
                                                 IUnitOfWork unitOfWork,
                                                 IRoomRepository roomRepository)
                                               : CommandHandler<CreateRoomCommand, CreateRoomResponse>(notificator)
    {
        public override async Task<Response<CreateRoomResponse>> ExecuteAsync(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if(!ExecuteValidation(new CreateRoomCommandValidator(), request))
                return Response<CreateRoomResponse>.Failure(GetNotifications());

            var room = request.MapToEntity();
            var result = await SaveRoomAsync(room);
            if (!result)
            {
                Notify(EReportMessages.FAIL_TO_PERSIST_DATA.GetEnumDescription());
                return Response<CreateRoomResponse>.Failure(GetNotifications());
            }

            return Response<CreateRoomResponse>.Success(new(room.Id), 201, EReportMessages.ROOM_CREATED_WITH_SUCCESS.GetEnumDescription());
        }

        private async Task<bool> SaveRoomAsync(Room room)
        {
            roomRepository.Create(room);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}
