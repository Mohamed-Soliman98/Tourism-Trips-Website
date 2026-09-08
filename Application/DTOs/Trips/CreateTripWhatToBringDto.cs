using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripWhatToBringDto
    {
        public LocalizedTextDto Description { get; set; } = new();
        public int DisplayOrder { get; set; }
    }
}
