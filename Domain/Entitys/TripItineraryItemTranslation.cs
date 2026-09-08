using Domain.Enum;
using System;

namespace Domain.Entitys
{
    public class TripItineraryItemTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Guid TripItineraryItemId { get; set; }
        public TripItineraryItem TripItineraryItem { get; set; } = null!;
    }
}
