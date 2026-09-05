using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class CreateTourTypeService : ICreateTourTypeService
    {
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTourTypeDto> _validator;

        public CreateTourTypeService(
            IUnitOfWork unitOfWork,
            ITourTypeRepository tourTypeRepository,
            IValidator<CreateTourTypeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tourTypeRepository = tourTypeRepository;
            _validator = validator;
        }

        public async Task<TourTypeCreatedResponseDto> CreateTourTypeAsync(
            CreateTourTypeDto dto,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            if (await _tourTypeRepository.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Tour type with name '{dto.Name.Trim()}' already exists.");
            }

            var tourType = new TourType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _tourTypeRepository.Add(tourType);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TourTypeCreatedResponseDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive,
                "Tour type created successfully.");
        }
    }
}
