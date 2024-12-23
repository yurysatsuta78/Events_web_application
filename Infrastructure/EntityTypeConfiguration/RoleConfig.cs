using Infrastructure.Entities;
using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class RoleConfig : IEntityTypeConfiguration<RoleDb>
    {
        public void Configure(EntityTypeBuilder<RoleDb> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasIndex(p => p.Name)
                .IsUnique();

            var roles = Enum.GetValues<Role>()
                .Select(r => new RoleDb
                {
                    Id = (int)r,
                    Name = r.ToString()
                });

            builder.HasData(roles);
        }
    }
}
