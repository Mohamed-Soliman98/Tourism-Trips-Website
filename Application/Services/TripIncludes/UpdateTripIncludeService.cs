using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripIncludes;
using FluentValidation;

namespace Application.Services.TripIncludes
{
    public class UpdateTripIncludeService : IUpdateTripIncludeService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripIncludeRepository _tripIncludeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripIncludeDto> _validator;

        public UpdateTripIncludeService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripIncludeRepository tripIncludeRepository,
            IValidator<UpdateTripIncludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripIncludeRepository = tripIncludeRepository;
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

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var include = await _tripIncludeRepository.GetByIdAsync(includeId, cancellationToken);
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
