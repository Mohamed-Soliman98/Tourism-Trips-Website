using Domain.Entity;
using System;
using System.Collections.Generic;

namespace Domain.Entitys
{
    public class TripExclude
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public ICollection<TripExcludeTranslation> Translations { get; set; } = new List<TripExcludeTranslation>();
    }
}
