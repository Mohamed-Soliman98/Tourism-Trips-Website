using Infrastructure.Configurations;
using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entity;

namespace Infrastructure.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(t => t.Slug)
                .IsUnique();

            builder.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(t => t.ShortDescription)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.LongDescription)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(t => t.MetaTitle)
                .HasMaxLength(200);

            builder.Property(t => t.MetaDescription)
                .HasMaxLength(500);
            builder.Property(t => t.AdultPrice)
                .HasPrecision(18, 2);

            builder.Property(t => t.ChildPrice)
                .HasPrecision(18, 2);

            builder.Property(t => t.OldPrice)
                .HasPrecision(18, 2);

            builder.Property(t => t.PickupLocation)
                .HasMaxLength(500);

            builder.Property(t => t.OgImage)
                .HasMaxLength(500);

            builder.HasOne(t => t.Category)
                .WithMany(c => c.Trips)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Destination)
                .WithMany(d => d.Trips)
                .HasForeignKey(t => t.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.TourType)
                .WithMany(tt => tt.Trips)
                .HasForeignKey(t => t.TourTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}