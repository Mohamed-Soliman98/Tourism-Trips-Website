using Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Trips
{
    public sealed record UpdateTripItineraryItemDto
    {
        public int DisplayOrder { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public sealed record UpdateTripIncludeDto
    {
        public string Description { get; set; } = string.Empty;
    }

    public sealed record UpdateTripExcludeDto
    {
        public string Description { get; set; } = string.Empty;
    }

    public sealed record UpdateFAQTranslationDto
    {
        public Language Language { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }

    public sealed record UpdateTripFAQDto
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public List<UpdateFAQTranslationDto> Translations { get; set; } = new();
    }

    public sealed record UpdateTripTranslationDto
    {
        public Language Language { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

    public sealed record UpdateTripDto
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public TripStatus Status { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public int Duration { get; set; }
        public DurationUnit DurationUnit { get; set; }
        public string? PickupLocation { get; set; }
        public string Currency { get; set; } = "EUR";

        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public bool IsPriceFrom { get; set; }

        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid TourTypeId { get; set; }

        public IFormFile? CoverImage { get; set; }
        public string? CoverImageAltText { get; set; }
        public List<IFormFile> GalleryImages { get; set; } = new();
        public IFormFile? OgImage { get; set; }
        public List<string>? GalleryAltTexts { get; set; }

        public List<UpdateTripItineraryItemDto> ItineraryItems { get; set; } = new();
        public List<UpdateTripIncludeDto> Includes { get; set; } = new();
        public List<UpdateTripExcludeDto> Excludes { get; set; } = new();
        public List<UpdateTripFAQDto> FAQs { get; set; } = new();
        public List<UpdateTripTranslationDto> Translations { get; set; } = new();
    }
}
