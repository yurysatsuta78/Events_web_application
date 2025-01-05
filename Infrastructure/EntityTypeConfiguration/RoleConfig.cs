using Domain.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(k => k.Id);

            builder.ToTable("Roles");

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(p => p.Name)
                .IsUnique();

            var roles = Enum.GetValues<Roles>()
                .Select(r => Role.Create((int)r, r.ToString()));

            builder.HasData(roles);
        }
    }
}