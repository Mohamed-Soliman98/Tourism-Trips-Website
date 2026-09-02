using Application.DTOs.TripImages;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripImages;
using FluentValidation;

namespace Application.Services.TripImages
{
    public class UpdateTripImageService : IUpdateTripImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripImageDto> _validator;

        public UpdateTripImageService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateTripImageDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripImageUpdatedResponseDto> UpdateTripImageAsync(Guid tripId,Guid imageId,UpdateTripImageDto dto,CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (imageId == Guid.Empty)
                throw new ArgumentException("Image Id cannot be empty.", nameof(imageId));

            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Load the image record (tracked — no AsNoTracking)
            var image = await _unitOfWork.TripImages.GetByIdAsync(imageId, cancellationToken);
            if (image == null)
                throw new KeyNotFoundException($"TripImage with ID '{imageId}' was not found.");

            // 3. Confirm the image belongs to the requested trip
            if (image.TripId != tripId)
                throw new InvalidOperationException(
                    $"Image '{imageId}' does not belong to trip '{tripId}'.");

            // 4. Enforce single IsCover business rule with two-phase save
            if (dto.IsCover && !image.IsCover)
            {
                // PHASE 1: Unset existing covers first to avoid unique constraint violation
                var currentCovers = await _unitOfWork.TripImages
                    .GetCoverImagesTrackedAsync(tripId, cancellationToken);
                
                foreach (var currentCover in currentCovers.Where(c => c.Id != imageId))
                {
                    currentCover.IsCover = false;
                    currentCover.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.TripImages.Update(currentCover);
                }
                
                // Save the "unset" changes first if there were any covers to unset
                if (currentCovers.Any(c => c.Id != imageId))
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            // 5. Apply changes to the target image
            image.AltText      = dto.AltText?.Trim();
            image.DisplayOrder = dto.DisplayOrder;
            image.IsCover      = dto.IsCover;
            image.UpdatedAt    = DateTime.UtcNow;

            // 6. Save the target image changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripImageUpdatedResponseDto(
                image.Id,
                image.TripId,
                image.ImageUrl,
                image.AltText,
                image.DisplayOrder,
                image.IsCover);
        }
    }
}
