using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripExcludes;
using FluentValidation;

namespace Application.Services.TripExcludes
{
    public class UpdateTripExcludeService : IUpdateTripExcludeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripExcludeDto> _validator;

        public UpdateTripExcludeService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateTripExcludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripExcludeUpdatedResponseDto> UpdateTripExcludeAsync(
            Guid tripId,
            Guid excludeId,
            UpdateTripExcludeDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (excludeId == Guid.Empty)
                throw new ArgumentException("Exclude Id cannot be empty.", nameof(excludeId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = await _unitOfWork.TripExcludes.GetByIdAsync(excludeId, cancellationToken);
            if (exclude == null)
                throw new KeyNotFoundException($"Trip exclude with ID '{excludeId}' was not found.");

            if (exclude.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip exclude '{excludeId}' does not belong to trip '{tripId}'.");

            exclude.Description = dto.Description.Trim();
            exclude.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripExcludeUpdatedResponseDto(
                exclude.Id,
                exclude.TripId,
                exclude.Description);
        }
    }
}
