using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripExcludes;
using Domain.Entitys;
using FluentValidation;

namespace Application.Services.TripExcludes
{
    public class AddTripExcludeService : IAddTripExcludeService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripExcludeRepository _tripExcludeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripExcludeDto> _validator;

        public AddTripExcludeService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripExcludeRepository tripExcludeRepository,
            IValidator<CreateTripExcludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripExcludeRepository = tripExcludeRepository;
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

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = new TripExclude
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                Description = dto.Description.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _tripExcludeRepository.Add(exclude);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripExcludeAddedResponseDto(
                exclude.Id,
                exclude.TripId,
                exclude.Description);
        }
    }
}
