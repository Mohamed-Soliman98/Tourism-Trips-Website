using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BookingInquiryConfiguration : IEntityTypeConfiguration<BookingInquiry>
    {
        public void Configure(EntityTypeBuilder<BookingInquiry> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.Phone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.WhatsApp)
                .HasMaxLength(50);

            builder.Property(b => b.Nationality)
                .HasMaxLength(100);

            builder.Property(b => b.HotelOrPickup)
                .HasMaxLength(300);

            builder.Property(b => b.SelectedDate)
                .IsRequired();

            builder.Property(b => b.Adults)
                .IsRequired();

            builder.Property(b => b.Children)
                .IsRequired();

            builder.Property(b => b.Notes)
                .HasMaxLength(2000);

            builder.Property(b => b.InternalNotes)
                .HasMaxLength(2000);

            builder.Property(b => b.Status)
                .IsRequired();

            builder.HasOne(b => b.Trip)
                .WithMany(t => t.BookingInquiries)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => b.TripId);
            builder.HasIndex(b => b.SelectedDate);
            builder.HasIndex(b => b.Status);
        }
    }
}
