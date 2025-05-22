using FluentValidation;

namespace Rooms.API.Application.Commands.Rooms.AddParticipant
{
    public sealed class AddParticipantValidator : AbstractValidator<AddParticipantCommand>
    {
        public AddParticipantValidator()
        {
        }
    }
}