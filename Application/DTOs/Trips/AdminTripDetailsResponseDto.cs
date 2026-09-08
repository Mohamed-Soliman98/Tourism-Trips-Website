using Domain.Enum;

namespace Application.DTOs.Trips
{
    // Admin DTOs - Full translation exposure for CMS

    public sealed record AdminTripTranslationDto(
        Guid Id,
        Language Language,
        string Title,
        string ShortDescription,
        string LongDescription,
        string? MetaTitle,
        string? MetaDescription,
        string? PickupLocation
    );

    public sealed record AdminItineraryItemTranslationDto(
        Guid Id,
        Language Language,
        string Title,
        string? Description
    );

    public sealed record AdminTripItineraryItemDto(
        Guid Id,
        int DisplayOrder,
        List<AdminItineraryItemTranslationDto> Translations
    );

    public sealed record AdminIncludeTranslationDto(
        Guid Id,
        Language Language,
        string Description
    );

    public sealed record AdminTripIncludeDto(
        Guid Id,
        List<AdminIncludeTranslationDto> Translations
    );

    public sealed record AdminExcludeTranslationDto(
        Guid Id,
        Language Language,
        string Description
    );

    public sealed record AdminTripExcludeDto(
        Guid Id,
        List<AdminExcludeTranslationDto> Translations
    );

    public sealed record AdminHighlightTranslationDto(
        Guid Id,
        Language Language,
        string Description
    );

    public sealed record AdminTripHighlightDto(
        Guid Id,
        int DisplayOrder,
        List<AdminHighlightTranslationDto> Translations
    );

    public sealed record AdminWhatToBringTranslationDto(
        Guid Id,
        Language Language,
        string Description
    );

    public sealed record AdminTripWhatToBringDto(
        Guid Id,
        int DisplayOrder,
        List<AdminWhatToBringTranslationDto> Translations
    );

    public sealed record AdminFAQTranslationDto(
        Guid Id,
        Language Language,
        string Question,
        string Answer
    );

    public sealed record AdminTripFAQDto(
        Guid Id,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive,
        List<AdminFAQTranslationDto> Translations
    );

    public sealed record AdminTripDetailsResponseDto(
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
        List<AdminTripItineraryItemDto> ItineraryItems,
        List<AdminTripIncludeDto> Includes,
        List<AdminTripExcludeDto> Excludes,
        List<AdminTripHighlightDto> Highlights,
        List<AdminTripWhatToBringDto> WhatToBringItems,
        List<AdminTripFAQDto> FAQs,
        List<AdminTripTranslationDto> Translations
    );
}
