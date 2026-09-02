using Application.DTOs.TripImages;

namespace Application.Interfaces.TripImages
{
    public interface IAddTripImageService
    {
        Task<TripImageAddedResponseDto> AddTripImageAsync(
            Guid tripId,
            AddTripImageDto dto,
            CancellationToken cancellationToken = default);
    }
}
