using Domain.Enum;
using Microsoft.AspNetCore.Http;


namespace Application.DTOs.Trips
{
    public sealed record CreateTripDto
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
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

        public string? Notes { get; set; }

        public List<CreateTripItineraryItemDto> ItineraryItems { get; set; } = new();
        public List<CreateTripIncludeDto> Includes { get; set; } = new();
        public List<CreateTripExcludeDto> Excludes { get; set; } = new();
        public List<CreateTripHighlightDto> Highlights { get; set; } = new();
        public List<CreateTripWhatToBringDto> WhatToBringItems { get; set; } = new();
        public List<CreateTripFAQDto> FAQs { get; set; } = new();
        public List<CreateTripTranslationDto> Translations { get; set; } = new();
    }
}
