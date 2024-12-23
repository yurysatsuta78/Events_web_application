using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities;

namespace Infrastructure
{
    public sealed class EventsDbContext : DbContext
    {
        public DbSet<EventDb> Events { get; set; }
        public DbSet<ParticipantDb> Participants { get; set; }
        public DbSet<ImageDb> Images { get; set; }
        public DbSet<RoleDb> Roles { get; set; }

        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
