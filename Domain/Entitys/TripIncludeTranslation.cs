using Domain.Enum;
using System;

namespace Domain.Entitys
{
    public class TripIncludeTranslation
    {
        public Guid Id { get; set; }
        public Language Language { get; set; }
        public string Description { get; set; } = string.Empty;

        public Guid TripIncludeId { get; set; }
        public TripInclude TripInclude { get; set; } = null!;
    }
}
