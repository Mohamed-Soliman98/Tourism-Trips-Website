using Domain.Enum;
using System;

namespace Domain.Entitys
{
    public class TripHighlightTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid TripHighlightId { get; set; }
        public TripHighlight TripHighlight { get; set; } = null!;
    }
}
