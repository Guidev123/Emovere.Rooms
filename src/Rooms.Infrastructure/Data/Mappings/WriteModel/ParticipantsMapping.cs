using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rooms.Domain.Entities;
using Rooms.Domain.ValueObjects;

namespace Rooms.Infrastructure.Data.Mappings.WriteModel
{
    internal sealed class ParticipantsMapping : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.ToTable("Participants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.RoomId).IsRequired();

            builder.OwnsOne(x => x.Email, email =>
            {
                email
                .Property(x => x.Address)
                .HasColumnName(nameof(Email))
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
            });
        }
    }
}
