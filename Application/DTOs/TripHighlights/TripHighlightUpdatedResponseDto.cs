namespace Application.DTOs.TripHighlights
{
    public sealed record TripHighlightUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip highlight updated successfully."
    );
}