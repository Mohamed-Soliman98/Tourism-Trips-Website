using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class TripIncludeConfiguration : IEntityTypeConfiguration<TripInclude>
    {
        public void Configure(EntityTypeBuilder<TripInclude> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(i => i.Trip)
                .WithMany(t => t.Includes)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
