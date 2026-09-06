using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripHighlightConfiguration : IEntityTypeConfiguration<TripHighlight>
    {
        public void Configure(EntityTypeBuilder<TripHighlight> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(h => h.DisplayOrder)
                .IsRequired();

            builder.HasOne(h => h.Trip)
                .WithMany(t => t.Highlights)
                .HasForeignKey(h => h.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
