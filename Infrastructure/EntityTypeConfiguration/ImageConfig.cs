using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class ImageConfig : IEntityTypeConfiguration<ImageDb>
    {
        public void Configure(EntityTypeBuilder<ImageDb> builder)
        {
            builder.HasKey(k => k.Id);

            builder.Property(p => p.ImagePath)
                .IsRequired();
            builder.HasIndex(p => p.ImagePath)
                .IsUnique();
        }
    }
}
