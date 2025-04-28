using Emovere.SharedKernel.Events;
using Microsoft.EntityFrameworkCore;
using Rooms.Domain.Entities;
using System.Reflection;

namespace Rooms.Infrastructure.Data.Contexts
{
    public sealed class RoomsWriteDbContext(DbContextOptions<RoomsWriteDbContext> options) : DbContext(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
