using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripExcludeConfiguration : IEntityTypeConfiguration<TripExclude>
    {
        public void Configure(EntityTypeBuilder<TripExclude> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Trip)
                .WithMany(t => t.Excludes)
                .HasForeignKey(e => e.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
