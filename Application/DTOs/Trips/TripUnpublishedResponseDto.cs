using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record TripUnpublishedResponseDto(
        Guid TripId,
        string Title,
        TripStatus Status,
        DateTime UpdatedAt,
        string Message = "Trip unpublished successfully."
    );
}