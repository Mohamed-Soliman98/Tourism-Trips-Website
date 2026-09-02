using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripIncludes;
using FluentValidation;

namespace Application.Services.TripIncludes
{
    public class UpdateTripIncludeService : IUpdateTripIncludeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripIncludeDto> _validator;

        public UpdateTripIncludeService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateTripIncludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripIncludeUpdatedResponseDto> UpdateTripIncludeAsync(
            Guid tripId,
            Guid includeId,
            UpdateTripIncludeDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (includeId == Guid.Empty)
                throw new ArgumentException("Include Id cannot be empty.", nameof(includeId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var include = await _unitOfWork.TripIncludes.GetByIdAsync(includeId, cancellationToken);
            if (include == null)
                throw new KeyNotFoundException($"Trip include with ID '{includeId}' was not found.");

            if (include.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip include '{includeId}' does not belong to trip '{tripId}'.");

            include.Description = dto.Description.Trim();
            include.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripIncludeUpdatedResponseDto(
                include.Id,
                include.TripId,
                include.Description);
        }
    }
}
