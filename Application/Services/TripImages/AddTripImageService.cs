using Application.DTOs.TripImages;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Storage;
using Application.Interfaces.TripImages;
using Domain.Entitys;
using FluentValidation;

namespace Application.Services.TripImages
{
    public class AddTripImageService : IAddTripImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;
        private readonly IValidator<AddTripImageDto> _validator;

        public AddTripImageService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage,
            IValidator<AddTripImageDto> validator)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
            _validator = validator;
        }

        public async Task<TripImageAddedResponseDto> AddTripImageAsync(
            Guid tripId,
            AddTripImageDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            string? uploadedPath = null;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                if (dto.IsCover)
                {
                    var currentCovers = await _unitOfWork.TripImages
                        .GetCoverImagesTrackedAsync(tripId, cancellationToken);

                    foreach (var currentCover in currentCovers)
                    {
                        currentCover.IsCover = false;
                        currentCover.UpdatedAt = DateTime.UtcNow;
                        _unitOfWork.TripImages.Update(currentCover);
                    }

                    if (currentCovers.Any())
                    {
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }

                uploadedPath = await _fileStorage.SaveAsync(dto.ImageFile!, "trips", cancellationToken);

                var tripImage = new TripImage
                {
                    Id           = Guid.NewGuid(),
                    TripId       = tripId,
                    ImageUrl     = uploadedPath,
                    AltText      = dto.AltText?.Trim(),
                    DisplayOrder = dto.DisplayOrder,
                    IsCover      = dto.IsCover,
                    CreatedAt    = DateTime.UtcNow
                };

                _unitOfWork.TripImages.Add(tripImage);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new TripImageAddedResponseDto(
                    tripImage.Id,
                    tripImage.TripId,
                    tripImage.ImageUrl,
                    tripImage.AltText,
                    tripImage.DisplayOrder,
                    tripImage.IsCover);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                if (uploadedPath is not null)
                    await _fileStorage.DeleteAsync(uploadedPath);

                throw;
            }
        }
    }
}
