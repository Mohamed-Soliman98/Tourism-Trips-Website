using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripImages;

namespace Application.Services.TripImages
{
    public class GetTripGalleryService : IGetTripGalleryService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripImageRepository _tripImageRepository;

        public GetTripGalleryService(
            ITripRepository tripRepository,
            ITripImageRepository tripImageRepository)
        {
            _tripRepository = tripRepository;
            _tripImageRepository = tripImageRepository;
        }

        public async Task<List<TripImageDto>> GetTripGalleryAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var images = await _tripImageRepository.GetByTripIdAsync(tripId, cancellationToken);

            return images
                .Select(i => new TripImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.AltText,
                    i.DisplayOrder,
                    i.IsCover))
                .OrderBy(i => i.DisplayOrder)
                .ToList();
        }
    }
}
