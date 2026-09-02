using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripIncludes
{
    public interface IAddTripIncludeService
    {
        Task<TripIncludeAddedResponseDto> AddTripIncludeAsync(
            Guid tripId,
            CreateTripIncludeDto dto,
            CancellationToken cancellationToken = default);
    }
}
