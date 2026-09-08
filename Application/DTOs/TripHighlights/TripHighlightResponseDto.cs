namespace Application.DTOs.TripHighlights
{
    public sealed record TripHighlightResponseDto(
        Guid Id,
        Guid TripId,
        string Description
    );
}