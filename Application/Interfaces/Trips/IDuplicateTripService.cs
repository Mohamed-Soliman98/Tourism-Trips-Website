using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IDuplicateTripService
    {
        Task<TripDuplicatedResponseDto> DuplicateTripAsync(Guid tripId, CancellationToken cancellationToken);
    }
}