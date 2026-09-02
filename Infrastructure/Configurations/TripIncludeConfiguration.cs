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
    public class TripIncludeConfiguration : IEntityTypeConfiguration<TripInclude>
    {
        public void Configure(EntityTypeBuilder<TripInclude> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(i => i.Trip)
                .WithMany(t => t.Includes)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
