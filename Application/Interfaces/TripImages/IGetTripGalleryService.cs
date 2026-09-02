using Application.DTOs.Trips;

namespace Application.Interfaces.TripImages
{
    public interface IGetTripGalleryService
    {
        Task<List<TripImageDto>> GetTripGalleryAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
