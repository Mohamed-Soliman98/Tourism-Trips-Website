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
    public class UpdateTripExcludeService : IUpdateTripExcludeService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripExcludeRepository _tripExcludeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripExcludeDto> _validator;

        public UpdateTripExcludeService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripExcludeRepository tripExcludeRepository,
            IValidator<UpdateTripExcludeDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripExcludeRepository = tripExcludeRepository;
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

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = await _tripExcludeRepository.GetByIdWithTranslationsAsync(excludeId, cancellationToken);
            if (exclude == null)
                throw new KeyNotFoundException($"Trip exclude with ID '{excludeId}' was not found.");

            if (exclude.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip exclude '{excludeId}' does not belong to trip '{tripId}'.");

            exclude.UpdatedAt = DateTime.UtcNow;

            var englishDescription = dto.Description.English?.Trim() ?? string.Empty;
            var germanDescription = dto.Description.German?.Trim();

            // Synchronize translations instead of deleting and recreating rows:
            // update the existing translation in place when present, add it when missing.
            var englishTranslation = exclude.Translations.FirstOrDefault(t => t.Language == Language.English);
            if (englishTranslation == null)
            {
                exclude.Translations.Add(new TripExcludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripExcludeId = exclude.Id,
                    Language = Language.English,
                    Description = englishDescription
                });
            }
            else
            {
                englishTranslation.Description = englishDescription;
            }

            var germanTranslation = exclude.Translations.FirstOrDefault(t => t.Language == Language.German);
            if (string.IsNullOrWhiteSpace(germanDescription))
            {
                // Language removed from the DTO: remove an existing German translation if present.
                if (germanTranslation != null)
                    exclude.Translations.Remove(germanTranslation);
            }
            else if (germanTranslation == null)
            {
                exclude.Translations.Add(new TripExcludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripExcludeId = exclude.Id,
                    Language = Language.German,
                    Description = germanDescription
                });
            }
            else
            {
                germanTranslation.Description = germanDescription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripExcludeUpdatedResponseDto(
                exclude.Id,
                exclude.TripId,
                englishDescription);
        }
    }
}