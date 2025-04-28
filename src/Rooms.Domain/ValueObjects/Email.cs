using Emovere.SharedKernel.DomainObjects;

namespace Rooms.Domain.ValueObjects
{
    public record Email : ValueObject
    {
        public string Address { get; }

        public Email(string address)
        {
            Address = address;
            Validate();
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureMatchesPattern(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", Address, "Invalid email format.");
        }
    }
}
