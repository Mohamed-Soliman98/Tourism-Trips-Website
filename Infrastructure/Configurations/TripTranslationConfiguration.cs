using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class TripTranslationConfiguration: IEntityTypeConfiguration<TripTranslation>
    {
        public void Configure(EntityTypeBuilder<TripTranslation> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Language)
                .IsRequired();

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

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

            builder.HasOne(t => t.Trip)
                .WithMany(trip => trip.Translations)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => new { t.TripId, t.Language })
                .IsUnique();
        }
    }
}
