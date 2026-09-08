using Application.DTOs.TripItineraryItems;

namespace Application.Interfaces.TripItineraryItems
{
    public interface IGetTripItineraryItemByIdService
    {
        Task<TripItineraryItemResponseDto> GetTripItineraryItemByIdAsync(
            Guid tripId,
            Guid itineraryItemId,
            CancellationToken cancellationToken = default);
    }
}