using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripItineraryItemConfiguration : IEntityTypeConfiguration<TripItineraryItem>
    {
        public void Configure(EntityTypeBuilder<TripItineraryItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Description)
                .HasMaxLength(1000);

            builder.HasOne(i => i.Trip)
                .WithMany(t => t.ItineraryItems)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        
    }
}