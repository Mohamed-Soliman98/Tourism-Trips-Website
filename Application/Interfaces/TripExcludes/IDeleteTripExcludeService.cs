using Application.DTOs.TripExcludes;

namespace Application.Interfaces.TripExcludes
{
    public interface IDeleteTripExcludeService
    {
        Task<TripExcludeDeletedResponseDto> DeleteTripExcludeAsync(
            Guid tripId,
            Guid excludeId,
            CancellationToken cancellationToken = default);
    }
}
