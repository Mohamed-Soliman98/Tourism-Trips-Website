using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripWhatToBringConfiguration : IEntityTypeConfiguration<TripWhatToBring>
    {
        public void Configure(EntityTypeBuilder<TripWhatToBring> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(w => w.DisplayOrder)
                .IsRequired();

            builder.HasOne(w => w.Trip)
                .WithMany(t => t.WhatToBringItems)
                .HasForeignKey(w => w.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
