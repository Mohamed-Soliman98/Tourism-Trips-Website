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
    public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
    {
        public void Configure(EntityTypeBuilder<FAQ> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Question)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(f => f.Answer)
                .IsRequired()
                .HasMaxLength(2000);
            builder.HasOne(f => f.Trip)
                .WithMany(t => t.FAQs)
                .HasForeignKey(f => f.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
