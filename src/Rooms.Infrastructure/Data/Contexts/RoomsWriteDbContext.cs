using Emovere.SharedKernel.Events;
using Microsoft.EntityFrameworkCore;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.Data.Contexts
{
    public sealed class RoomsWriteDbContext(DbContextOptions<RoomsWriteDbContext> options) : DbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfiguration(new Mappings.WriteModel.RoomsMapping());
            modelBuilder.ApplyConfiguration(new Mappings.WriteModel.ParticipantsMapping());
        }
    }
}
