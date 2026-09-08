namespace Application.DTOs.TripItineraryItems
{
    public sealed record TripItineraryItemAddedResponseDto(
        Guid Id,
        Guid TripId,
        int DisplayOrder,
        string Title,
        string? Description,
        string Message = "Trip itinerary item added successfully."
    );
}