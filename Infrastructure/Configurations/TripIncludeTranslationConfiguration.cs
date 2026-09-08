using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripIncludeTranslationConfiguration : IEntityTypeConfiguration<TripIncludeTranslation>
    {
        public void Configure(EntityTypeBuilder<TripIncludeTranslation> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Language)
                .IsRequired();

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(t => t.TripInclude)
                .WithMany(i => i.Translations)
                .HasForeignKey(t => t.TripIncludeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.TripIncludeId, t.Language })
                .IsUnique();
        }
    }
}
