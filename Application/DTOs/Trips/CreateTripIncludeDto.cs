namespace Application.DTOs.Trips
{
    public sealed record CreateTripIncludeDto
    {
        public string Description { get; set; } = string.Empty;
    }
}
