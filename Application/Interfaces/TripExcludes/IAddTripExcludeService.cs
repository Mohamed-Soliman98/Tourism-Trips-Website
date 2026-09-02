using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripExcludes
{
    public interface IAddTripExcludeService
    {
        Task<TripExcludeAddedResponseDto> AddTripExcludeAsync(
            Guid tripId,
            CreateTripExcludeDto dto,
            CancellationToken cancellationToken = default);
    }
}
