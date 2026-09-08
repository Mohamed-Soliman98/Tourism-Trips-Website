using Application.DTOs.TripHighlights;

namespace Application.Interfaces.TripHighlights
{
    public interface IGetTripHighlightByIdService
    {
        Task<TripHighlightResponseDto> GetTripHighlightByIdAsync(
            Guid tripId,
            Guid highlightId,
            CancellationToken cancellationToken = default);
    }
}