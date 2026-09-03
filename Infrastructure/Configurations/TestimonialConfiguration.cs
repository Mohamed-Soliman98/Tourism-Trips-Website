using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
    {
        public void Configure(EntityTypeBuilder<Testimonial> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Country)
                .HasMaxLength(100);

            builder.Property(t => t.Rating)
                .IsRequired();

            builder.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.ToTable(t => t.HasCheckConstraint("CK_Testimonial_Rating_Range", "[Rating] >= 1 AND [Rating] <= 5"));
        }
    }
}