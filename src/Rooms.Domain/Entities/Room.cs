using Emovere.SharedKernel.DomainObjects;
using Rooms.Domain.Enums;
using Rooms.Domain.ValueObjects;

namespace Rooms.Domain.Entities
{
    public class Room : Entity, IAggregateRoot
    {
        public const int MAX_STANDARD_PARTICIPANTS = 50;
        public const int MAX_PREMIUM_PARTICIPANTS = 150;
        public const int MAX_VIP_PARTICIPANTS = 500;
        public const int MAX_EXCLUSIVE_PARTICIPANTS = 1000;

        public Room(Guid hostId, string name, string details, DateTime startDate, DateTime? endDate = null)
        {
            HostId = hostId;
            StartDate = startDate;
            EndDate = endDate;
            Plan = ERoomPlan.Standard;
            Status = ERoomStatus.InProgress;
            RoomSpecification = new(name, details, GetMaxParticipantsByPlan());
            Validate();
        }

        protected Room()
        { }

        public Guid HostId { get; private set; }
        public RoomSpecification RoomSpecification { get; private set; } = default!;
        public ERoomPlan Plan { get; private set; }
        public ERoomStatus Status { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public decimal? Price { get; private set; }
        public int ParticipantsQuantity { get; private set; }
        public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

        private readonly List<Participant> _participants = [];

        public void UpdateEndDate(DateTime endDate)
        {
            AssertionConcern.EnsureTrue(endDate > StartDate, "End date must be greater than start date.");
            EndDate = endDate;
        }

        public void CloseRoom()
        {
            VerifiyRoomIsInProgress();

            Status = ERoomStatus.Closed;
        }

        public void SetPlan(ERoomPlan plan)
        {
            VerifiyRoomIsInProgress();

            Plan = plan;
            UpdateRoomSpecification(RoomSpecification.Name, RoomSpecification.Description);
        }

        internal void AddParticipant(Participant participant)
        {
            _participants.Add(participant);
            GetCurrentParticipantsQuantity();

            VerifyParticipantQuantity();
        }

        internal void RemoveParticipant(Participant participant)
        {
            if (!_participants.Contains(participant))
                throw new DomainException("Participant not found in the room.");

            _participants.Remove(participant);
            GetCurrentParticipantsQuantity();
        }

        internal void UpdateRoomSpecification(string name, string details)
        {
            VerifiyRoomIsInProgress();

            RoomSpecification = new(name, details, GetMaxParticipantsByPlan());
        }

        private int GetMaxParticipantsByPlan()
            => Plan switch
            {
                ERoomPlan.Standard => MAX_STANDARD_PARTICIPANTS,
                ERoomPlan.Premium => MAX_PREMIUM_PARTICIPANTS,
                ERoomPlan.Vip => MAX_VIP_PARTICIPANTS,
                ERoomPlan.Exclusive => MAX_EXCLUSIVE_PARTICIPANTS,
                _ => throw new DomainException("Unknown room plan.")
            };

        private void VerifyParticipantQuantity()
        {
            if (ParticipantsQuantity > RoomSpecification.MaxParticipantsNumber)
                throw new DomainException("Participant quantity exceeded the maximum limit.");
        }

        private void VerifiyRoomIsInProgress()
        {
            if (Status is ERoomStatus.Closed)
                throw new DomainException("Room is already closed.");
        }

        private void GetCurrentParticipantsQuantity()
            => ParticipantsQuantity = _participants.Count;

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(HostId, Guid.Empty, "HostId cannot be empty.");
            AssertionConcern.EnsureNotNull(RoomSpecification, "Room Specification cannot be null.");

            if (EndDate.HasValue)
                AssertionConcern.EnsureTrue(EndDate > StartDate, "End Date must be after Start Date.");

            AssertionConcern.EnsureTrue(StartDate > DateTime.UtcNow, "Start Date must be in the future.");
        }
    }
}
