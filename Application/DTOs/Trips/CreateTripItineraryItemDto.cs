using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripItineraryItemDto
    {
        public int DisplayOrder { get; set; }
        public LocalizedTextDto Title { get; set; } = new();
        public LocalizedTextDto? Description { get; set; }
    }
}
