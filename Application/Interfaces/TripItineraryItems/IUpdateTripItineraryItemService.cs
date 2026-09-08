using Application.DTOs.TripItineraryItems;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripItineraryItems
{
    public interface IUpdateTripItineraryItemService
    {
        Task<TripItineraryItemUpdatedResponseDto> UpdateTripItineraryItemAsync(
            Guid tripId,
            Guid itineraryItemId,
            UpdateTripItineraryItemDto dto,
            CancellationToken cancellationToken = default);
    }
}