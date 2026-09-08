using Application.DTOs.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripDto
    {
        public LocalizedTextDto Title { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public int Duration { get; set; }
        public DurationUnit DurationUnit { get; set; }
        public LocalizedTextDto? PickupLocation { get; set; }
        public string Currency { get; set; } = "EUR";

        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public bool IsPriceFrom { get; set; }

        public LocalizedTextDto ShortDescription { get; set; } = new();
        public LocalizedTextDto LongDescription { get; set; } = new();
        public LocalizedTextDto? MetaTitle { get; set; }
        public LocalizedTextDto? MetaDescription { get; set; }

        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
        public Guid TourTypeId { get; set; }

        public string? Notes { get; set; }

        public List<CreateTripItineraryItemDto> ItineraryItems { get; set; } = new();
        public List<CreateTripIncludeDto> Includes { get; set; } = new();
        public List<CreateTripExcludeDto> Excludes { get; set; } = new();
        public List<CreateTripHighlightDto> Highlights { get; set; } = new();
        public List<CreateTripWhatToBringDto> WhatToBringItems { get; set; } = new();
        public List<CreateTripFAQDto> FAQs { get; set; } = new();
    }
}
