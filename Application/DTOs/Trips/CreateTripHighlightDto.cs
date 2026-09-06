namespace Application.DTOs.Trips
{
    public sealed record CreateTripHighlightDto
    {
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
