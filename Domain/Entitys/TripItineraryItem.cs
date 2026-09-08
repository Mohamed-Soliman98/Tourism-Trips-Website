using Domain.Entity;
using System;
using System.Collections.Generic;

namespace Domain.Entitys
{
    public class TripItineraryItem
    {
        public Guid Id { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public ICollection<TripItineraryItemTranslation> Translations { get; set; } = new List<TripItineraryItemTranslation>();
    }
}
