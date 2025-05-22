using Emovere.Communication.IntegrationEvents;
using Emovere.SharedKernel.Abstractions;
using Emovere.SharedKernel.Notifications;
using Emovere.SharedKernel.Responses;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Interfaces.Services;

namespace Rooms.API.Application.Commands.Rooms.AddParticipant
{
    public sealed class AddParticipantHandler(INotificator notificator,
                                              IRoomRepository roomRepository,
                                              IUnitOfWork unitOfWork,
                                              IRoomCapacityValidationService roomCapacityValidation) : CommandHandler<AddParticipantCommand, AddParticipantResponse>(notificator)
    {
        public override async Task<Response<AddParticipantResponse>> ExecuteAsync(AddParticipantCommand request, CancellationToken cancellationToken)
        {
            if (!ExecuteValidation(new AddParticipantValidator(), request))
                return Response<AddParticipantResponse>.Failure(Notifications);

            var room = await roomRepository.GetByIdAsync(request.RoomId).ConfigureAwait(false);
            if (room is null)
            {
                Notify(EReportMessages.USER_NOT_FOUND.GetEnumDescription());
                return Response<AddParticipantResponse>.Failure(Notifications);
            }

            var resultIsSuccess = await roomCapacityValidation.AddParticipantAsync(new Participant(request.CustomerId, room.Id, request.Email), room);
            if (!resultIsSuccess)
                return Response<AddParticipantResponse>.Failure(Notifications);

            room.AddEvent(new ParticipantAddedIntegrationEvent(room.Id, request.CustomerId, request.Email));

            var persistData = await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            if (!persistData)
            {
                Notify(EReportMessages.FAIL_TO_PERSIST_DATA.GetEnumDescription());
                return Response<AddParticipantResponse>.Failure(Notifications);
            }

            return Response<AddParticipantResponse>.Success(default, StatusCode.NO_CONTENT_STATUS_CODE);
        }
    }
}