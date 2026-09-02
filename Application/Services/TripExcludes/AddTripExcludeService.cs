using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripExcludes;
using Domain.Entitys;
using FluentValidation;

namespace Application.Services.TripExcludes
{
    public class AddTripExcludeService : IAddTripExcludeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripExcludeDto> _validator;

        public AddTripExcludeService(
            IUnitOfWork unitOfWork,
            IValidator<CreateTripExcludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripExcludeAddedResponseDto> AddTripExcludeAsync(
            Guid tripId,
            CreateTripExcludeDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = new TripExclude
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                Description = dto.Description.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.TripExcludes.Add(exclude);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripExcludeAddedResponseDto(
                exclude.Id,
                exclude.TripId,
                exclude.Description);
        }
    }
}
