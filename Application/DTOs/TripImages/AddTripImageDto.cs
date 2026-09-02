using Microsoft.AspNetCore.Http;

namespace Application.DTOs.TripImages
{
    /// <summary>
    /// Request DTO for adding a new image to a trip gallery.
    /// TripId comes from the route — not included here.
    /// </summary>
    public sealed record AddTripImageDto
    {
        /// <summary>The image file to upload.</summary>
        public IFormFile? ImageFile { get; init; }

        /// <summary>Optional alt text for accessibility / SEO.</summary>
        public string? AltText { get; init; }

        /// <summary>Controls the display position in the gallery (0-based, ascending).</summary>
        public int DisplayOrder { get; init; }

        /// <summary>When true this image is treated as the gallery cover (distinct from Trip.CoverImage).</summary>
        public bool IsCover { get; init; }
    }
}
