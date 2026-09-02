namespace Application.DTOs.TripIncludes
{
    public sealed record TripIncludeUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip include updated successfully."
    );
}
