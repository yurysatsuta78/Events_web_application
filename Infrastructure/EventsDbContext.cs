using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure
{
    public sealed class EventsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Role> Roles { get; set; }

        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
