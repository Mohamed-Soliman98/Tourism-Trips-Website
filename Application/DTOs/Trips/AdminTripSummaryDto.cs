using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record AdminTripSummaryDto(
        Guid Id,
        string Title,
        string Slug,
        TripStatus Status,
        bool IsFeatured,
        int DisplayOrder,
        int Duration,
        DurationUnit DurationUnit,
        string Currency,
        decimal AdultPrice,
        decimal ChildPrice,
        decimal? OldPrice,
        bool IsPriceFrom,
        string? CoverImage,
        string? CoverImageAltText,
        CategoryDto Category,
        DestinationDto Destination,
        TourTypeDto TourType,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
