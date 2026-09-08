namespace Application.DTOs.TripHighlights
{
    public sealed record TripHighlightDeletedResponseDto(
        string Message = "Trip highlight deleted successfully."
    );
}