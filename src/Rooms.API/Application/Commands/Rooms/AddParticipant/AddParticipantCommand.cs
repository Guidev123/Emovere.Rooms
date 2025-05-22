using Emovere.SharedKernel.Abstractions;

namespace Rooms.API.Application.Commands.Rooms.AddParticipant
{
    public record AddParticipantCommand(
        Guid CustomerId,
        Guid RoomId,
        string Email
        ) : Command<AddParticipantResponse>;
}