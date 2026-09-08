using Domain.Entity;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitys
{
    public class TripTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? PickupLocation { get; set; }

        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;
    }
}
