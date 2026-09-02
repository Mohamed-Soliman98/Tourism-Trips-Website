using Application.DTOs.TripIncludes;

namespace Application.Interfaces.TripIncludes
{
    public interface IGetTripIncludesService
    {
        Task<List<TripIncludeResponseDto>> GetTripIncludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
