using Microsoft.EntityFrameworkCore;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.Data.Contexts
{
    public sealed class RoomsReadDbContext(DbContextOptions<RoomsReadDbContext> options) : DbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Mappings.ReadModel.RoomsMapping());
            modelBuilder.ApplyConfiguration(new Mappings.ReadModel.ParticipantsMapping());
        }
    }
}
