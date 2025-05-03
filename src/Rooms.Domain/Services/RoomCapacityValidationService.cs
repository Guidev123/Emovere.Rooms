using Emovere.SharedKernel.Notifications;
using Rooms.Domain.Entities;
using Rooms.Domain.Enums;
using Rooms.Domain.Interfaces.Repositories;
using Rooms.Domain.Interfaces.Services;
using Rooms.Domain.Strategies.Factories;

namespace Rooms.Domain.Services
{
    public sealed class RoomCapacityValidationService(IAddParticipantStrategyFactory strategyFactory,
                                           INotificator notificator,
                                           IRoomRepository roomRepository,
                                           IUnitOfWork unitOfWork)
                                         : IRoomCapacityValidationService
    {
        public async Task<bool> AddParticipantAsync(Participant participant, Room room)
        {
            var participantAddedSuccessfully = strategyFactory
                .GetStrategy(room.Plan)
                .AddParticipant(participant, room);

            if (!participantAddedSuccessfully)
            {
                notificator.HandleNotification(new(EReportMessages.PARTICIPANT_CANNOT_BE_ADDED_TO_ROOM.GetEnumDescription()));
                return false;
            }

            roomRepository.Update(room);
            return await unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> RemoveParticipantAsync(Participant participant, Room room)
        {
            room.RemoveParticipant(participant);

            roomRepository.Update(room);
            return await unitOfWork.SaveChangesAsync();
        }
    }
}