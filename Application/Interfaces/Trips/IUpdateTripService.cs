using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IUpdateTripService
    {
        Task<TripUpdatedResponseDto> UpdateTripAsync(Guid id, UpdateTripDto dto, CancellationToken cancellationToken = default);
    }
}
