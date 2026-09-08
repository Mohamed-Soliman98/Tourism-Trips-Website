using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record CategoryDto(
        Guid Id,
        string Name
    );

    public sealed record DestinationDto(
        Guid Id,
        string Name
    );

    public sealed record TourTypeDto(
        Guid Id,
        string Name
    );

    public sealed record TripImageDto(
        Guid Id,
        string ImageUrl,
        string? AltText,
        int DisplayOrder,
        bool IsCover
    );

    public sealed record TripItineraryItemDto(
        Guid Id,
        int DisplayOrder,
        string Title,
        string? Description
    );

    public sealed record TripIncludeDto(
        Guid Id,
        string Description
    );

    public sealed record TripExcludeDto(
        Guid Id,
        string Description
    );

    public sealed record TripHighlightDto(
        Guid Id,
        string Description,
        int DisplayOrder
    );

    public sealed record TripWhatToBringDto(
        Guid Id,
        string Description,
        int DisplayOrder
    );

    public sealed record FAQTranslationDto(
        Guid Id,
        Language Language,
        string Question,
        string Answer
    );

    // Obsolete - use PublicTripFAQDto for public endpoints or AdminTripFAQDto for admin endpoints
    public sealed record TripFAQDto(
        Guid Id,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive
    );

    public sealed record TripTranslationDto(
        Guid Id,
        Language Language,
        string Title,
        string ShortDescription,
        string LongDescription,
        string? MetaTitle,
        string? MetaDescription,
        string? PickupLocation
    );

    // Obsolete - use PublicTripDetailsResponseDto for public endpoints or AdminTripDetailsResponseDto for admin endpoints
    public sealed record TripDetailsResponseDto(
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
        List<TripFAQDto> FAQs
    );
}
