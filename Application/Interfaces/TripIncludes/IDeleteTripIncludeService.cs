using Application.DTOs.TripIncludes;

namespace Application.Interfaces.TripIncludes
{
    public interface IDeleteTripIncludeService
    {
        Task<TripIncludeDeletedResponseDto> DeleteTripIncludeAsync(
            Guid tripId,
            Guid includeId,
            CancellationToken cancellationToken = default);
    }
}
