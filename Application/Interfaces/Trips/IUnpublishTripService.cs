using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IUnpublishTripService
    {
        Task<TripUnpublishedResponseDto> UnpublishTripAsync(Guid tripId, CancellationToken cancellationToken);
    }
}