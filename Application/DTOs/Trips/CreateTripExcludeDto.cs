namespace Application.DTOs.Trips
{
    public sealed record CreateTripExcludeDto
    {
        public string Description { get; set; } = string.Empty;
    }
}
