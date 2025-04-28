using Emovere.SharedKernel.DomainObjects;

namespace Rooms.Domain.ValueObjects
{
    public record RoomSpecification : ValueObject
    {
        public string Name { get; }
        public string Description { get; }
        public int MaxParticipantsNumber { get; }

        public RoomSpecification(string name, string description, int maxParticipantsQuantity)
        {
            Name = name;
            Description = description;
            MaxParticipantsNumber = maxParticipantsQuantity;
            Validate();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotEmpty(Name, "Name cannot be empty.");
            AssertionConcern.EnsureNotEmpty(Description, "Description cannot be empty.");
            AssertionConcern.EnsureGreaterThan(MaxParticipantsNumber, 0, "Max participants number must be greater than 0.");
        }
    }
}
