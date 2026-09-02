using Application.DTOs.TripImages;

namespace Application.Interfaces.TripImages
{
    public interface IDeleteTripImageService
    {
        Task<TripImageDeletedResponseDto> DeleteTripImageAsync(
            Guid tripId,
            Guid imageId,
            CancellationToken cancellationToken = default);
    }
}
