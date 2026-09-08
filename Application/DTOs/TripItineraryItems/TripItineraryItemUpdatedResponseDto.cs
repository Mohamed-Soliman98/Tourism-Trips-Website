namespace Application.DTOs.TripItineraryItems
{
    public sealed record TripItineraryItemUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        int DisplayOrder,
        string Title,
        string? Description,
        string Message = "Trip itinerary item updated successfully."
    );
}