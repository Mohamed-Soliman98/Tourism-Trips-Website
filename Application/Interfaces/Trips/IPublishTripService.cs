using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IPublishTripService
    {
        Task<TripPublishedResponseDto> PublishTripAsync(Guid tripId, CancellationToken cancellationToken);
    }
}