using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripItineraryItemTranslationConfiguration : IEntityTypeConfiguration<TripItineraryItemTranslation>
    {
        public void Configure(EntityTypeBuilder<TripItineraryItemTranslation> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Language)
                .IsRequired();

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(1000);

            builder.HasOne(t => t.TripItineraryItem)
                .WithMany(i => i.Translations)
                .HasForeignKey(t => t.TripItineraryItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.TripItineraryItemId, t.Language })
                .IsUnique();
        }
    }
}
