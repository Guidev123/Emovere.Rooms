using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.Data.Mappings.WriteModel
{
    internal sealed class RoomsMapping : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");

            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.RoomSpecification, roomsSpecification =>
            {
                roomsSpecification.Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)").IsRequired();

                roomsSpecification.Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("VARCHAR(256)").IsRequired();

                roomsSpecification.Property(x => x.MaxParticipantsNumber)
                .HasColumnName("MaxParticipantsNumber")
                .IsRequired();
            });

            builder.HasMany(x => x.Participants)
                .WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId);

            builder.Property(x => x.Price).HasColumnType("MONEY").IsRequired(false);

            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired(false);
            builder.Property(x => x.ParticipantsQuantity).IsRequired();
        }
    }
}
