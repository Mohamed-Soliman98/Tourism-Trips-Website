using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class UpdateTourTypeService : IUpdateTourTypeService
    {
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTourTypeDto> _validator;

        public UpdateTourTypeService(
            IUnitOfWork unitOfWork,
            ITourTypeRepository tourTypeRepository,
            IValidator<UpdateTourTypeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tourTypeRepository = tourTypeRepository;
            _validator = validator;
        }

        public async Task<TourTypeUpdatedResponseDto> UpdateTourTypeAsync(
            Guid id,
            UpdateTourTypeDto dto,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tour type Id cannot be empty.", nameof(id));
            }

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var tourType = await _tourTypeRepository.GetByIdAsync(id, cancellationToken);
            if (tourType == null)
            {
                throw new KeyNotFoundException($"Tour type with ID '{id}' was not found.");
            }

            if (await _tourTypeRepository.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A tour type with the name '{dto.Name.Trim()}' already exists.");
            }

            tourType.Name = dto.Name.Trim();
            tourType.IsActive = dto.IsActive;
            tourType.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TourTypeUpdatedResponseDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive
            );
        }
    }
}
