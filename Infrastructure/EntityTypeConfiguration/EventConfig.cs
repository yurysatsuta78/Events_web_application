using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(k => k.Id);

            builder.ToTable("Events");

            builder.Navigation(nameof(Event.Participants)).UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.Navigation(nameof(Event.Images)).UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(e => e.Images)
                .WithOne()
                .HasForeignKey("EventId").IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Participants)
                .WithMany()
                .UsingEntity<EventParticipant>(
                    e => e.HasOne<Participant>().WithMany().HasForeignKey(k => k.ParticipantId),
                    p => p.HasOne<Event>().WithMany().HasForeignKey(k => k.EventId),
                    j => {
                        j.Property(p => p.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        j.ToTable("EventParticipants");
                    });

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.Property(e => e.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(e => e.EventTime)
                .IsRequired();

            builder.Property(e => e.Location)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Category)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.MaxParticipants)
                .HasMaxLength(5)
                .IsRequired();
        }
    }
}