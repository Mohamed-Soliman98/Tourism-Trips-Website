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
    public class FAQTranslationConfiguration:IEntityTypeConfiguration<FAQTranslation>
    {
        public void Configure(EntityTypeBuilder<FAQTranslation> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Language)
                .IsRequired();

            builder.Property(f => f.Question)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(f => f.Answer)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasOne(f => f.FAQ)
                .WithMany(faq => faq.Translations)
                .HasForeignKey(f => f.FAQId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => new { f.FAQId, f.Language })
                .IsUnique();
        }
    }
}
