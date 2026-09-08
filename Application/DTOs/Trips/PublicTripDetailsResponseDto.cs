using Domain.Enum;

namespace Application.DTOs.Trips
{
    // Public DTOs - No translation collections exposed
    
    public sealed record PublicTripFAQDto(
        Guid Id,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive
    );

    public sealed record PublicTripDetailsResponseDto(
        Guid Id,
        string Title,
        string Slug,
        TripStatus Status,
        bool IsFeatured,
        int DisplayOrder,
        int Duration,
        DurationUnit DurationUnit,
        string? PickupLocation,
        string Currency,
        decimal AdultPrice,
        decimal ChildPrice,
        decimal? OldPrice,
        bool IsPriceFrom,
        string ShortDescription,
        string LongDescription,
        string? MetaTitle,
        string? MetaDescription,
        string? OgImage,
        string? CoverImage,
        string? CoverImageAltText,
        string? Notes,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        CategoryDto Category,
        DestinationDto Destination,
        TourTypeDto TourType,
        List<TripImageDto> Images,
        List<TripItineraryItemDto> ItineraryItems,
        List<TripIncludeDto> Includes,
        List<TripExcludeDto> Excludes,
        List<TripHighlightDto> Highlights,
        List<TripWhatToBringDto> WhatToBringItems,
        List<PublicTripFAQDto> FAQs
    );
}
