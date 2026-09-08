using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripWhatToBringTranslationConfiguration : IEntityTypeConfiguration<TripWhatToBringTranslation>
    {
        public void Configure(EntityTypeBuilder<TripWhatToBringTranslation> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Language)
                .IsRequired();

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(t => t.TripWhatToBring)
                .WithMany(w => w.Translations)
                .HasForeignKey(t => t.TripWhatToBringId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.TripWhatToBringId, t.Language })
                .IsUnique();
        }
    }
}
