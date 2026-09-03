using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(b => b.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.ButtonText)
                .HasMaxLength(100);

            builder.Property(b => b.ButtonUrl)
                .HasMaxLength(500);

            builder.Property(b => b.DisplayOrder)
                .IsRequired();

            builder.Property(b => b.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasIndex(b => new { b.IsActive, b.DisplayOrder });
        }
    }
}