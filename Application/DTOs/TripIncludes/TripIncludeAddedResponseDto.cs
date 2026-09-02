namespace Application.DTOs.TripIncludes
{
    public sealed record TripIncludeAddedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip include added successfully."
    );
}
