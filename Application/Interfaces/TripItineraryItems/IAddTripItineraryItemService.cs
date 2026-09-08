using Application.DTOs.TripItineraryItems;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripItineraryItems
{
    public interface IAddTripItineraryItemService
    {
        Task<TripItineraryItemAddedResponseDto> AddTripItineraryItemAsync(
            Guid tripId,
            CreateTripItineraryItemDto dto,
            CancellationToken cancellationToken = default);
    }
}