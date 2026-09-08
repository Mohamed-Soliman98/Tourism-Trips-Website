using Application.DTOs.Common;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripFAQDto
    {
        public LocalizedTextDto Question { get; set; } = new();
        public LocalizedTextDto Answer { get; set; } = new();
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
