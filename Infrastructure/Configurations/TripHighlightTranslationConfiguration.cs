using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripHighlightTranslationConfiguration : IEntityTypeConfiguration<TripHighlightTranslation>
    {
        public void Configure(EntityTypeBuilder<TripHighlightTranslation> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Language)
                .IsRequired();

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(t => t.TripHighlight)
                .WithMany(h => h.Translations)
                .HasForeignKey(t => t.TripHighlightId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.TripHighlightId, t.Language })
                .IsUnique();
        }
    }
}
