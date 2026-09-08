using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripHighlightDto
    {
        public LocalizedTextDto Description { get; set; } = new();
        public int DisplayOrder { get; set; }
    }
}
