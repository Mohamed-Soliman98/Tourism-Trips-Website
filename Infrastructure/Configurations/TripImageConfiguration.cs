using Infrastructure.Configurations;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripImageConfiguration : IEntityTypeConfiguration<TripImage>
    {
        public void Configure(EntityTypeBuilder<TripImage> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.AltText)
                .HasMaxLength(200);

            builder.HasOne(i => i.Trip)
                .WithMany(t => t.Images)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasIndex(i => i.TripId)
                .IsUnique()
                .HasFilter("[IsCover] = 1")
                .HasDatabaseName("IX_TripImages_TripId_IsCover_Unique");
        }
    }
}
