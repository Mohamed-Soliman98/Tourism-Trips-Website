using Application.DTOs.TripHighlights;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripHighlights
{
    public interface IAddTripHighlightService
    {
        Task<TripHighlightAddedResponseDto> AddTripHighlightAsync(
            Guid tripId,
            CreateTripHighlightDto dto,
            CancellationToken cancellationToken = default);
    }
}