using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record TripPublishedResponseDto(
        Guid TripId,
        string Title,
        TripStatus Status,
        DateTime UpdatedAt,
        string Message = "Trip published successfully."
    );
}