using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripExcludes;
using Domain.Entitys;
using Domain.Enum;
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
                CreatedAt = DateTime.UtcNow
            };

            exclude.Translations.Add(new TripExcludeTranslation
            {
                Id = Guid.NewGuid(),
                TripExcludeId = exclude.Id,
                Language = Language.English,
                Description = dto.Description.English?.Trim() ?? string.Empty
            });

            if (!string.IsNullOrWhiteSpace(dto.Description.German))
            {
                exclude.Translations.Add(new TripExcludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripExcludeId = exclude.Id,
                    Language = Language.German,
                    Description = dto.Description.German.Trim()
                });
            }

            _tripExcludeRepository.Add(exclude);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the English description from the translation for the response
            var englishTranslation = exclude.Translations.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? string.Empty;

            return new TripExcludeAddedResponseDto(
                exclude.Id,
                exclude.TripId,
                description);
        }
    }
}