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
    public class TripExcludeConfiguration : IEntityTypeConfiguration<TripExclude>
    {
        public void Configure(EntityTypeBuilder<TripExclude> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(e => e.Trip)
                .WithMany(t => t.Excludes)
                .HasForeignKey(e => e.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
