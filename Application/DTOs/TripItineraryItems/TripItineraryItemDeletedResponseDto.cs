namespace Application.DTOs.TripItineraryItems
{
    public sealed record TripItineraryItemDeletedResponseDto(
        string Message = "Trip itinerary item deleted successfully."
    );
}