using Application.DTOs.TripImages;

namespace Application.Interfaces.TripImages
{
    public interface IUpdateTripImageService
    {
        Task<TripImageUpdatedResponseDto> UpdateTripImageAsync(
            Guid tripId,
            Guid imageId,
            UpdateTripImageDto dto,
            CancellationToken cancellationToken = default);
    }
}
