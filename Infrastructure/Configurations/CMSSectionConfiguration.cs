using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CMSSectionConfiguration : IEntityTypeConfiguration<CMSSection>
    {
        public void Configure(EntityTypeBuilder<CMSSection> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(500);

            builder.Property(c => c.DisplayOrder)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // Create unique index for Key to prevent duplicates
            builder.HasIndex(c => c.Key)
                .IsUnique();

            // Create index for efficient ordering queries
            builder.HasIndex(c => new { c.IsActive, c.DisplayOrder });
        }
    }
}