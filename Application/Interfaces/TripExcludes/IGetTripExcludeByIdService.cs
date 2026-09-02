using Application.DTOs.TripExcludes;

namespace Application.Interfaces.TripExcludes
{
    public interface IGetTripExcludeByIdService
    {
        Task<TripExcludeResponseDto> GetTripExcludeByIdAsync(
            Guid tripId,
            Guid excludeId,
            CancellationToken cancellationToken = default);
    }
}
