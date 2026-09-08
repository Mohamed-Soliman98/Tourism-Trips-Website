using Application.DTOs.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Application.DTOs.Trips
{
    public sealed record UpdateTripItineraryItemDto
    {
        public int DisplayOrder { get; set; }
        public LocalizedTextDto Title { get; set; } = new();
        public LocalizedTextDto? Description { get; set; }
    }

    public sealed record UpdateTripIncludeDto
    {
        public LocalizedTextDto Description { get; set; } = new();
    }

    public sealed record UpdateTripExcludeDto
    {
        public LocalizedTextDto Description { get; set; } = new();
    }

    public sealed record UpdateTripHighlightDto
    {
        public LocalizedTextDto Description { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public sealed record UpdateTripWhatToBringDto
    {
        public LocalizedTextDto Description { get; set; } = new();
        public int DisplayOrder { get; set; }
    }

    public sealed record UpdateTripFAQDto
    {
        public LocalizedTextDto Question { get; set; } = new();
        public LocalizedTextDto Answer { get; set; } = new();
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed record UpdateTripDto
    {
        public LocalizedTextDto Title { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public TripStatus Status { get; set; }
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

        public List<UpdateTripItineraryItemDto> ItineraryItems { get; set; } = new();
        public List<UpdateTripIncludeDto> Includes { get; set; } = new();
        public List<UpdateTripExcludeDto> Excludes { get; set; } = new();
        public List<UpdateTripHighlightDto> Highlights { get; set; } = new();
        public List<UpdateTripWhatToBringDto> WhatToBringItems { get; set; } = new();
        public List<UpdateTripFAQDto> FAQs { get; set; } = new();
    }
}
