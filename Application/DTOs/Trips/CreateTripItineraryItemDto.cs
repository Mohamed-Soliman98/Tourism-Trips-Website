namespace Application.DTOs.Trips
{
    public sealed record CreateTripItineraryItemDto
    {
        public int DisplayOrder { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
