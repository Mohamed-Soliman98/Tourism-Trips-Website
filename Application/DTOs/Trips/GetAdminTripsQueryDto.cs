using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record GetAdminTripsQueryDto
    {
        public string? Search { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? DestinationId { get; set; }
        public Guid? TourTypeId { get; set; }
        public TripStatus? Status { get; set; }
        public bool? IsFeatured { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = "DisplayOrder";
        public string? SortDirection { get; set; } = "asc";
    }
}
