using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
    {
        public void Configure(EntityTypeBuilder<SiteSetting> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.WhatsApp)
                .HasMaxLength(20);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.FacebookUrl)
                .HasMaxLength(500);

            builder.Property(s => s.InstagramUrl)
                .HasMaxLength(500);

            builder.Property(s => s.YouTubeUrl)
                .HasMaxLength(500);

            builder.Property(s => s.TikTokUrl)
                .HasMaxLength(500);

            builder.Property(s => s.DefaultMetaTitle)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.DefaultMetaDescription)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

        }
    }
}