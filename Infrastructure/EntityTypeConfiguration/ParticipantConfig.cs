using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class ParticipantConfig : IEntityTypeConfiguration<ParticipantDb>
    {
        public void Configure(EntityTypeBuilder<ParticipantDb> builder)
        {
            builder.HasKey(k => k.Id);

            builder.HasMany(e => e.Roles)
                .WithMany(e => e.Participants)
                .UsingEntity<ParticipantRoleDb>(
                    r => r.HasOne<RoleDb>().WithMany().HasForeignKey(k => k.RoleId),
                    p => p.HasOne<ParticipantDb>().WithMany().HasForeignKey(k => k.ParticipantId),
                    j => j.ToTable("ParticipantRoles")
                );

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
