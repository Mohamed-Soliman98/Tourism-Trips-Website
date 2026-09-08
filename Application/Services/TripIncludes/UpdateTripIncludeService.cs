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

            var include = await _tripIncludeRepository.GetByIdWithTranslationsAsync(includeId, cancellationToken);
            if (include == null)
                throw new KeyNotFoundException($"Trip include with ID '{includeId}' was not found.");

            if (include.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip include '{includeId}' does not belong to trip '{tripId}'.");

            include.UpdatedAt = DateTime.UtcNow;

            var englishDescription = dto.Description.English?.Trim() ?? string.Empty;
            var germanDescription = dto.Description.German?.Trim();

            // Synchronize translations instead of deleting and recreating rows:
            // update the existing translation in place when present, add it when missing.
            var englishTranslation = include.Translations.FirstOrDefault(t => t.Language == Language.English);
            if (englishTranslation == null)
            {
                include.Translations.Add(new TripIncludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripIncludeId = include.Id,
                    Language = Language.English,
                    Description = englishDescription
                });
            }
            else
            {
                englishTranslation.Description = englishDescription;
            }

            var germanTranslation = include.Translations.FirstOrDefault(t => t.Language == Language.German);
            if (string.IsNullOrWhiteSpace(germanDescription))
            {
                // Language removed from the DTO: remove an existing German translation if present.
                if (germanTranslation != null)
                    include.Translations.Remove(germanTranslation);
            }
            else if (germanTranslation == null)
            {
                include.Translations.Add(new TripIncludeTranslation
                {
                    Id = Guid.NewGuid(),
                    TripIncludeId = include.Id,
                    Language = Language.German,
                    Description = germanDescription
                });
            }
            else
            {
                germanTranslation.Description = germanDescription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripIncludeUpdatedResponseDto(
                include.Id,
                include.TripId,
                englishDescription);
        }
    }
}