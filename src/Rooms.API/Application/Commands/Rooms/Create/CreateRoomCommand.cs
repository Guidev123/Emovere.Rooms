using Emovere.SharedKernel.Abstractions;

namespace Rooms.API.Application.Commands.Rooms.Create
{
    public record CreateRoomCommand : Command<CreateRoomResponse>
    {
        public CreateRoomCommand(string name, string details, DateTime startDate, DateTime? endDate = null)
        {
            Name = name;
            Details = details;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guid HostId { get; private set; }
        public string Name { get; } = string.Empty;
        public string Details { get; } = string.Empty;
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }

        public void SetHostId(Guid hostId)
        {
            AggregateId = hostId;
            HostId = hostId;
        }
    }
}