using Domain.Entity;
using System;
using System.Collections.Generic;

namespace Domain.Entitys
{
    public class TripWhatToBring
    {
        public Guid Id { get; set; }
        public int DisplayOrder { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public ICollection<TripWhatToBringTranslation> Translations { get; set; } = new List<TripWhatToBringTranslation>();
    }
}
