using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripIncludeDto
    {
        public LocalizedTextDto Description { get; set; } = new();
    }
}
