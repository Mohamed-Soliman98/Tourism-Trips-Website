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
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            if (await _unitOfWork.Destinations.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Destination with name '{dto.Name.Trim()}' already exists.");
            }

            var destination = new Domain.Entity.Destination
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Destinations.Add(destination);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DestinationCreatedResponseDto(
                destination.Id,
                destination.Name,
                destination.IsActive,
                "Destination created successfully.");
        }
    }
}