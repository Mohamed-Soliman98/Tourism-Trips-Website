using Application.DTOs.Destinations;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Destinations;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.Destinations
{
    public class CreateDestinationService : ICreateDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateDestinationDto> _validator;

        public CreateDestinationService(
            IUnitOfWork unitOfWork,
            IValidator<CreateDestinationDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<DestinationCreatedResponseDto> CreateDestinationAsync(
            CreateDestinationDto dto,
            CancellationToken cancellationToken = default)
        {
            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Check Name Uniqueness
            if (await _unitOfWork.Destinations.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Destination with name '{dto.Name.Trim()}' already exists.");
            }

            // 3. Instantiate Destination Entity
            var destination = new Domain.Entity.Destination
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Add via Generic Repository
            _unitOfWork.Destinations.Add(destination);

            // 5. Commit Changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6. Return Response DTO
            return new DestinationCreatedResponseDto(
                destination.Id,
                destination.Name,
                destination.IsActive,
                "Destination created successfully.");
        }
    }
}