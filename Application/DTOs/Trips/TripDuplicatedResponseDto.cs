using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record TripDuplicatedResponseDto(
        Guid TripId,
        string Title,
        string Slug,
        TripStatus Status,
        DateTime CreatedAt,
        string Message = "Trip duplicated successfully."
    );
}