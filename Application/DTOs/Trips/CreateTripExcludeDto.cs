using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripExcludeDto
    {
        public LocalizedTextDto Description { get; set; } = new();
    }
}
