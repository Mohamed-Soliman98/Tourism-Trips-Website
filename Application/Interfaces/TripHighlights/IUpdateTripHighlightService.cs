using Application.DTOs.TripHighlights;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripHighlights
{
    public interface IUpdateTripHighlightService
    {
        Task<TripHighlightUpdatedResponseDto> UpdateTripHighlightAsync(
            Guid tripId,
            Guid highlightId,
            UpdateTripHighlightDto dto,
            CancellationToken cancellationToken = default);
    }
}