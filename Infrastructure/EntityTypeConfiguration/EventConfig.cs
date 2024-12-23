using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class EventConfig : IEntityTypeConfiguration<EventDb>
    {
        public void Configure(EntityTypeBuilder<EventDb> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasMany(e => e.Images)
                .WithOne(e => e.Event)
                .HasForeignKey(k => k.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Participants)
                .WithMany(e => e.Events)
                .UsingEntity<EventParticipantDb>(
                    e => e.HasOne<ParticipantDb>().WithMany().HasForeignKey(k => k.ParticipantId),
                    p => p.HasOne<EventDb>().WithMany().HasForeignKey(k => k.EventId),
                    j => {
                        j.Property(p => p.EventRegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                        j.ToTable("EventParticipants");
                    }
                );

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
