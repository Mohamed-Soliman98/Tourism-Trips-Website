using Application.DTOs.TripHighlights;

namespace Application.Interfaces.TripHighlights
{
    public interface IDeleteTripHighlightService
    {
        Task<TripHighlightDeletedResponseDto> DeleteTripHighlightAsync(
            Guid tripId,
            Guid highlightId,
            CancellationToken cancellationToken = default);
    }
}