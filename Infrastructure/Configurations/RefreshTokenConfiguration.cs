using Domain.Entitys;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.Property(rt => rt.UserId)
                .IsRequired();

            builder.Property(rt => rt.DeviceName)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(rt => rt.Browser)
              .HasMaxLength(100)
              .IsRequired(false);

            builder.Property(rt => rt.OperatingSystem)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(rt => rt.IpAddress)
                .HasMaxLength(45)
                .IsRequired(false);

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .IsRequired(false);

            // Relationship: RefreshToken belongs to ApplicationUser
            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
