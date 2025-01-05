using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class ParticipantConfig : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(k => k.Id);

            builder.ToTable("Participants");

            builder.HasMany(e => e.Roles)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(j => 
                {
                    j.Property<Guid>("ParticipantId").IsRequired();
                    j.Property<int>("RoleId").IsRequired();
                    j.HasKey("ParticipantId", "RoleId");

                    j.HasOne<Participant>().WithMany().HasForeignKey("ParticipantId");
                    j.HasOne<Role>().WithMany().HasForeignKey("RoleId");
                    j.ToTable("ParticipantRoles");
                });

            builder.Metadata.FindNavigation(nameof(Participant.Roles))?.SetField("_roles");

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.BirthDay)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(p => p.Email)
                .IsUnique();

            builder.Property(p => p.PasswordHash)
                .IsRequired();
        }
    }
}