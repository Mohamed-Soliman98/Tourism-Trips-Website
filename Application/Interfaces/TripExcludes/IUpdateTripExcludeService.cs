using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripExcludes
{
    public interface IUpdateTripExcludeService
    {
        Task<TripExcludeUpdatedResponseDto> UpdateTripExcludeAsync(
            Guid tripId,
            Guid excludeId,
            UpdateTripExcludeDto dto,
            CancellationToken cancellationToken = default);
    }
}
