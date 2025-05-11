using Microsoft.EntityFrameworkCore;
using Rooms.Domain.Entities;

namespace Rooms.Infrastructure.Data.Contexts
{
    public sealed class RoomsReadDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public RoomsReadDbContext(DbContextOptions<RoomsReadDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Mappings.ReadModel.RoomsMapping());
            modelBuilder.ApplyConfiguration(new Mappings.ReadModel.ParticipantsMapping());
        }
    }
}