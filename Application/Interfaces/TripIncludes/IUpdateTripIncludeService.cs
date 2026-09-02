using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripIncludes
{
    public interface IUpdateTripIncludeService
    {
        Task<TripIncludeUpdatedResponseDto> UpdateTripIncludeAsync(
            Guid tripId,
            Guid includeId,
            UpdateTripIncludeDto dto,
            CancellationToken cancellationToken = default);
    }
}
