using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;
using FluentValidation;

namespace Application.Services.Destinations
{
    public class UpdateDestinationService : IUpdateDestinationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateDestinationDto> _validator;

        public UpdateDestinationService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateDestinationDto> validator)
        {
            _unitOfWork = unitOfWork;
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

            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Load existing destination — reuses Generic Repository: GetByIdAsync
            var destination = await _unitOfWork.Destinations.GetByIdAsync(id, cancellationToken);
            if (destination == null)
            {
                throw new KeyNotFoundException($"Destination with ID '{id}' was not found.");
            }

            // 3. Check name uniqueness (exclude current record)
            if (await _unitOfWork.Destinations.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A destination with the name '{dto.Name.Trim()}' already exists.");
            }

            // 4. Apply changes
            destination.Name = dto.Name.Trim();
            destination.IsActive = dto.IsActive;
            destination.UpdatedAt = DateTime.UtcNow;

            // 5. Persist
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DestinationUpdatedResponseDto(
                destination.Id,
                destination.Name,
                destination.IsActive
            );
        }
    }
}
