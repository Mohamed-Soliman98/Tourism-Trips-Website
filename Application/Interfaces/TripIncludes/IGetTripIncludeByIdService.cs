using Application.DTOs.TripIncludes;

namespace Application.Interfaces.TripIncludes
{
    public interface IGetTripIncludeByIdService
    {
        Task<TripIncludeResponseDto> GetTripIncludeByIdAsync(
            Guid tripId,
            Guid includeId,
            CancellationToken cancellationToken = default);
    }
}
