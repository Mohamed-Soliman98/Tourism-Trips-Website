using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripIncludes;
using Domain.Entitys;
using Domain.Enum;
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
                CreatedAt = DateTime.UtcNow
            };

            include.Translations.Add(new TripIncludeTranslation
            {
                Id = Guid.NewGuid(),
                TripIncludeId = include.Id,
                Language = Language.English,
                Description = dto.Description.English?.Trim() ?? string.Empty
            });

            if (!string.IsNullOrWhiteSpace(dto.Description.German))
            {
                include.Translations.Add(new TripIncludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripIncludeId = include.Id,
                    Language = Language.German,
                    Description = dto.Description.German.Trim()
                });
            }

            _tripIncludeRepository.Add(include);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the English description from the translation for the response
            var englishTranslation = include.Translations.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? string.Empty;

            return new TripIncludeAddedResponseDto(
                include.Id,
                include.TripId,
                description);
        }
    }
}