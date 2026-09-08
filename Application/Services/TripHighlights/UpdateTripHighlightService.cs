using Application.DTOs.TripHighlights;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripHighlights;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.TripHighlights
{
    public class UpdateTripHighlightService : IUpdateTripHighlightService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHighlightRepository _tripHighlightRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripHighlightDto> _validator;

        public UpdateTripHighlightService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripHighlightRepository tripHighlightRepository,
            IValidator<UpdateTripHighlightDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripHighlightRepository = tripHighlightRepository;
            _validator = validator;
        }

        public async Task<TripHighlightUpdatedResponseDto> UpdateTripHighlightAsync(
            Guid tripId,
            Guid highlightId,
            UpdateTripHighlightDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (highlightId == Guid.Empty)
                throw new ArgumentException("Highlight Id cannot be empty.", nameof(highlightId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var highlight = await _tripHighlightRepository.GetByIdWithTranslationsAsync(highlightId, cancellationToken);
            if (highlight == null)
                throw new KeyNotFoundException($"Trip highlight with ID '{highlightId}' was not found.");

            if (highlight.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip highlight '{highlightId}' does not belong to trip '{tripId}'.");

            highlight.DisplayOrder = dto.DisplayOrder;

            var englishDescription = dto.Description.English?.Trim() ?? string.Empty;
            var germanDescription = dto.Description.German?.Trim();

            var englishTranslation = highlight.Translations.FirstOrDefault(t => t.Language == Language.English);
            if (englishTranslation == null)
            {
                highlight.Translations.Add(new TripHighlightTranslation
                {
                    Id = Guid.NewGuid(),
                    TripHighlightId = highlight.Id,
                    Language = Language.English,
                    Description = englishDescription
                });
            }
            else
            {
                englishTranslation.Description = englishDescription;
            }

            var germanTranslation = highlight.Translations.FirstOrDefault(t => t.Language == Language.German);
            if (string.IsNullOrWhiteSpace(germanDescription))
            {
                if (germanTranslation != null)
                    highlight.Translations.Remove(germanTranslation);
            }
            else if (germanTranslation == null)
            {
                highlight.Translations.Add(new TripHighlightTranslation
                {
                    Id = Guid.NewGuid(),
                    TripHighlightId = highlight.Id,
                    Language = Language.German,
                    Description = germanDescription
                });
            }
            else
            {
                germanTranslation.Description = germanDescription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripHighlightUpdatedResponseDto(
                highlight.Id,
                highlight.TripId,
                englishDescription);
        }
    }
}