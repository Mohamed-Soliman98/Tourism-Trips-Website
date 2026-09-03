using Microsoft.AspNetCore.Http;

namespace Application.DTOs.TripImages
{
    public sealed record AddTripImageDto
    {
        public IFormFile? ImageFile { get; init; }

        public string? AltText { get; init; }

        public int DisplayOrder { get; init; }

        public bool IsCover { get; init; }
    }
}
