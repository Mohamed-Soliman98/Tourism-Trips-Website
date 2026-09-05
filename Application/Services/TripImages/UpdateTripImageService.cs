using Application.DTOs.TripImages;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripImages;
using FluentValidation;

namespace Application.Services.TripImages
{
    public class UpdateTripImageService : IUpdateTripImageService
    {
        private readonly ITripImageRepository _tripImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripImageDto> _validator;

        public UpdateTripImageService(
            IUnitOfWork unitOfWork,
            ITripImageRepository tripImageRepository,
            IValidator<UpdateTripImageDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripImageRepository = tripImageRepository;
            _validator = validator;
        }

        public async Task<TripImageUpdatedResponseDto> UpdateTripImageAsync(Guid tripId,Guid imageId,UpdateTripImageDto dto,CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (imageId == Guid.Empty)
                throw new ArgumentException("Image Id cannot be empty.", nameof(imageId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var image = await _tripImageRepository.GetByIdAsync(imageId, cancellationToken);
            if (image == null)
                throw new KeyNotFoundException($"TripImage with ID '{imageId}' was not found.");

            if (image.TripId != tripId)
                throw new InvalidOperationException(
                    $"Image '{imageId}' does not belong to trip '{tripId}'.");

            if (dto.IsCover && !image.IsCover)
            {
                var currentCovers = await _tripImageRepository
                    .GetCoverImagesTrackedAsync(tripId, cancellationToken);
                
                foreach (var currentCover in currentCovers.Where(c => c.Id != imageId))
                {
                    currentCover.IsCover = false;
                    currentCover.UpdatedAt = DateTime.UtcNow;
                    _tripImageRepository.Update(currentCover);
                }
                
                if (currentCovers.Any(c => c.Id != imageId))
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            image.AltText      = dto.AltText?.Trim();
            image.DisplayOrder = dto.DisplayOrder;
            image.IsCover      = dto.IsCover;
            image.UpdatedAt    = DateTime.UtcNow;

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
