namespace Application.DTOs.TripHighlights
{
    public sealed record TripHighlightAddedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip highlight added successfully."
    );
}