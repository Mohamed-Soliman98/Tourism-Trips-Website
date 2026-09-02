using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripImages;

namespace Application.Services.TripImages
{
    public class GetTripGalleryService : IGetTripGalleryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripGalleryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TripImageDto>> GetTripGalleryAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            // Verify the trip exists before returning an empty list
            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var images = await _unitOfWork.TripImages.GetByTripIdAsync(tripId, cancellationToken);

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
