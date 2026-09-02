namespace Application.DTOs.Trips
{
    public sealed record GetPublicTripsQueryDto
    {
        public string? Search { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? DestinationId { get; set; }
        public bool? IsFeatured { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
