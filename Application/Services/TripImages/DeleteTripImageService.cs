using Application.DTOs.TripImages;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Storage;
using Application.Interfaces.TripImages;

namespace Application.Services.TripImages
{
    public class DeleteTripImageService : IDeleteTripImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;

        public DeleteTripImageService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task<TripImageDeletedResponseDto> DeleteTripImageAsync(
            Guid tripId,
            Guid imageId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (imageId == Guid.Empty)
                throw new ArgumentException("Image Id cannot be empty.", nameof(imageId));

            var image = await _unitOfWork.TripImages.GetByIdAsync(imageId, cancellationToken);
            if (image == null)
                throw new KeyNotFoundException($"TripImage with ID '{imageId}' was not found.");

            if (image.TripId != tripId)
                throw new InvalidOperationException(
                    $"Image '{imageId}' does not belong to trip '{tripId}'.");

            var imagePath = image.ImageUrl;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _unitOfWork.TripImages.Remove(image);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            if (!string.IsNullOrWhiteSpace(imagePath))
                await _fileStorage.DeleteAsync(imagePath);

            return new TripImageDeletedResponseDto();
        }
    }
}
