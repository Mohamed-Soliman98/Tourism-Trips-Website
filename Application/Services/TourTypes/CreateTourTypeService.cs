using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class CreateTourTypeService : ICreateTourTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTourTypeDto> _validator;

        public CreateTourTypeService(
            IUnitOfWork unitOfWork,
            IValidator<CreateTourTypeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TourTypeCreatedResponseDto> CreateTourTypeAsync(
            CreateTourTypeDto dto,
            CancellationToken cancellationToken = default)
        {
            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Check Name Uniqueness
            if (await _unitOfWork.TourTypes.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Tour type with name '{dto.Name.Trim()}' already exists.");
            }

            // 3. Instantiate TourType Entity
            var tourType = new TourType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Add via Generic Repository
            _unitOfWork.TourTypes.Add(tourType);

            // 5. Commit Changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Return Response DTO
            return new TourTypeCreatedResponseDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive,
                "Tour type created successfully.");
        }
    }
}
