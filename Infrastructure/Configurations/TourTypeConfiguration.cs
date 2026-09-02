using Infrastructure.Configurations;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TourTypeConfiguration : IEntityTypeConfiguration<TourType>
    {
        public void Configure(EntityTypeBuilder<TourType> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}