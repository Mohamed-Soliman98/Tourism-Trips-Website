using Application.DTOs.TripItineraryItems;

namespace Application.Interfaces.TripItineraryItems
{
    public interface IDeleteTripItineraryItemService
    {
        Task<TripItineraryItemDeletedResponseDto> DeleteTripItineraryItemAsync(
            Guid tripId,
            Guid itineraryItemId,
            CancellationToken cancellationToken = default);
    }
}