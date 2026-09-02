using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Storage;
using Application.Interfaces.Trips;

namespace Application.Services.Trips
{
    public class DeleteTripService : IDeleteTripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;

        public DeleteTripService(IUnitOfWork unitOfWork, IFileStorageService fileStorage)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task<TripDeletedResponseDto> DeleteTripAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // 1. Validate ID
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Trip Id cannot be empty.", nameof(id));
            }

            // 2. Retrieve existing Trip with child entities
            var trip = await _unitOfWork.Trips.GetByIdForDeleteAsync(id, cancellationToken);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID '{id}' was not found.");
            }

            // 3. Collect all physical file references belonging to the Trip
            var physicalFilesToDelete = new List<string>();

            if (!string.IsNullOrWhiteSpace(trip.OgImage))
            {
                physicalFilesToDelete.Add(trip.OgImage);
            }

            foreach (var img in trip.Images)
            {
                if (!string.IsNullOrWhiteSpace(img.ImageUrl))
                {
                    physicalFilesToDelete.Add(img.ImageUrl);
                }
            }

            // 4. Begin Database Transaction and Remove Trip
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                _unitOfWork.Trips.Remove(trip);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            // 5. Delete physical files from file storage ONLY AFTER transaction commits successfully
            foreach (var filePath in physicalFilesToDelete)
            {
                await _fileStorage.DeleteAsync(filePath);
            }

            return new TripDeletedResponseDto("Trip deleted successfully.");
        }
    }
}
