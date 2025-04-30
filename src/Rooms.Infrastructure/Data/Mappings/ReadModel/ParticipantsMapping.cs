using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.Data.Mappings.ReadModel
{
    internal sealed class ParticipantsMapping : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToCollection("Participants");

            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id)
                .HasConversion(guid => guid.ToString(), str => Guid.Parse(str))
                .IsRequired();

            builder.Property(c => c.CustomerId)
                .HasConversion(guid => guid.ToString(), str => Guid.Parse(str))
                .IsRequired();

            builder.Property(c => c.RoomId)
                .HasConversion(guid => guid.ToString(), str => Guid.Parse(str))
                .IsRequired();

            builder.Ignore(x => x.Events);

            builder.OwnsOne(p => p.Email, emailBuilder =>
            {
                emailBuilder.Property(e => e.Address)
                    .HasElementName("email")
                    .IsRequired();
            });
        }
    }
}
