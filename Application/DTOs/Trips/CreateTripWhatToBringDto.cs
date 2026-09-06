namespace Application.DTOs.Trips
{
    public sealed record CreateTripWhatToBringDto
    {
        public string Description { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
