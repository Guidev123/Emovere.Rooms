using Emovere.SharedKernel.DomainObjects;
using Rooms.Domain.Enums;
using Rooms.Domain.ValueObjects;

namespace Rooms.Domain.Entities
{
    public class Participant : Entity
    {
        public Participant(Guid customerId, Guid roomId, string email)
        {
            CustomerId = customerId;
            RoomId = roomId;
            Email = new(email);
            Validate();
        }

        public Guid CustomerId { get; private set; }
        public Guid RoomId { get; private set; }
        public Email Email { get; private set; } = default!;
        public EParticipantStatus Status { get; private set; }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(CustomerId, Guid.Empty, "CustomerId cannot be empty.");
            AssertionConcern.EnsureDifferent(RoomId, Guid.Empty, "RoomId cannot be empty.");
            AssertionConcern.EnsureNotNull(Email, "Email cannot be null.");
        }
    }
}
