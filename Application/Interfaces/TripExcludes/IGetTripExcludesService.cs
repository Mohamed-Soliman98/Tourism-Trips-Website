using Application.DTOs.TripExcludes;

namespace Application.Interfaces.TripExcludes
{
    public interface IGetTripExcludesService
    {
        Task<List<TripExcludeResponseDto>> GetTripExcludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
