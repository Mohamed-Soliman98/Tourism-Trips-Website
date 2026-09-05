using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripIncludes;
using Domain.Entitys;
using FluentValidation;

namespace Application.Services.TripIncludes
{
    public class AddTripIncludeService : IAddTripIncludeService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripIncludeRepository _tripIncludeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripIncludeDto> _validator;

        public AddTripIncludeService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripIncludeRepository tripIncludeRepository,
            IValidator<CreateTripIncludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripIncludeRepository = tripIncludeRepository;
            _validator = validator;
        }

        public async Task<TripIncludeAddedResponseDto> AddTripIncludeAsync(
            Guid tripId,
            CreateTripIncludeDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var include = new TripInclude
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                Description = dto.Description.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _tripIncludeRepository.Add(include);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripIncludeAddedResponseDto(
                include.Id,
                include.TripId,
                include.Description);
        }
    }
}
