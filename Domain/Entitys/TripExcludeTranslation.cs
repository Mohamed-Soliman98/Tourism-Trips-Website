using Domain.Enum;
using System;

namespace Domain.Entitys
{
    public class TripExcludeTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid TripExcludeId { get; set; }
        public TripExclude TripExclude { get; set; } = null!;
    }
}
