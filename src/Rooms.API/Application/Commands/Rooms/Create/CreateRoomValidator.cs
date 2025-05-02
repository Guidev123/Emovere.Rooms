using FluentValidation;

namespace Rooms.API.Application.Commands.Rooms.Create
{
    public sealed class CreateRoomValidator : AbstractValidator<CreateRoomCommand>
    {
        private const int MIN_START_DATE_IN_FUTURE = 30;

        public CreateRoomValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Length(2, 50)
                .WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.Details)
                .NotEmpty()
                .WithMessage("Details are required.")
                .Length(10, 500)
                .WithMessage("Details must be between 10 and 500 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required.")
                .GreaterThan(DateTime.UtcNow.AddMinutes(MIN_START_DATE_IN_FUTURE))
                .WithMessage($"The start date must be {MIN_START_DATE_IN_FUTURE} minutes in the future.");
        }
    }
}