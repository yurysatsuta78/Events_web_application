using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(k => k.Id);

            builder.ToTable("Images");

            builder.Property<Guid>("EventId")
                .IsRequired();

            builder.Property(p => p.ImagePath)
                .IsRequired();
            builder.HasIndex(p => p.ImagePath)
                .IsUnique();
        }
    }
}