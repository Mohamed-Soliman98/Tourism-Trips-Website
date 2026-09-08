using Application.DTOs.TripItineraryItems;

namespace Application.Interfaces.TripItineraryItems
{
    public interface IGetTripItineraryItemsService
    {
        Task<List<TripItineraryItemResponseDto>> GetTripItineraryItemsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}