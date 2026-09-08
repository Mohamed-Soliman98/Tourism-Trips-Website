using Domain.Enum;
using System;

namespace Domain.Entitys
{
    public class TripWhatToBringTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid TripWhatToBringId { get; set; }
        public TripWhatToBring TripWhatToBring { get; set; } = null!;
    }
}
