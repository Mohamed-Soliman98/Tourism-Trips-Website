using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Services.Destinations
{
    public class UpdateDestinationService : IUpdateDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateDestinationDto> _validator;

        public UpdateDestinationService(
            IUnitOfWork unitOfWork,
            IDestinationRepository destinationRepository,
            IValidator<UpdateDestinationDto> validator)
        {
            _unitOfWork = unitOfWork;
            _destinationRepository = destinationRepository;
            _validator = validator;
        }

        public async Task<DestinationUpdatedResponseDto> UpdateDestinationAsync(
            Guid id,
            UpdateDestinationDto dto,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Destination Id cannot be empty.", nameof(id));
            }

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var destination = await _destinationRepository.GetByIdAsync(id, cancellationToken);
            if (destination == null)
            {
                throw new KeyNotFoundException($"Destination with ID '{id}' was not found.");
            }

            if (await _destinationRepository.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A destination with the name '{dto.Name.Trim()}' already exists.");
            }

            destination.Name = dto.Name.Trim();
            destination.IsActive = dto.IsActive;
            destination.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DestinationUpdatedResponseDto(
                destination.Id,
                destination.Name,
                destination.IsActive
            );
        }
    }
}
