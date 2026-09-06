using Domain.Entity;
using System;

namespace Domain.Entitys
{
    public class TripWhatToBring
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;
    }
}
