using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class UpdateTourTypeService : IUpdateTourTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTourTypeDto> _validator;

        public UpdateTourTypeService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateTourTypeDto> validator)
        {
            _unitOfWork = unitOfWork;
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

            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Load existing tour type — reuses Generic Repository: GetByIdAsync
            var tourType = await _unitOfWork.TourTypes.GetByIdAsync(id, cancellationToken);
            if (tourType == null)
            {
                throw new KeyNotFoundException($"Tour type with ID '{id}' was not found.");
            }

            // 3. Check name uniqueness (exclude current record)
            if (await _unitOfWork.TourTypes.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A tour type with the name '{dto.Name.Trim()}' already exists.");
            }

            // 4. Apply changes
            tourType.Name = dto.Name.Trim();
            tourType.IsActive = dto.IsActive;
            tourType.UpdatedAt = DateTime.UtcNow;

            // 5. Persist
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TourTypeUpdatedResponseDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive
            );
        }
    }
}
