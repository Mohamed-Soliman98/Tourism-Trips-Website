using Application.DTOs.TripHighlights;

namespace Application.Interfaces.TripHighlights
{
    public interface IGetTripHighlightsService
    {
        Task<List<TripHighlightResponseDto>> GetTripHighlightsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}