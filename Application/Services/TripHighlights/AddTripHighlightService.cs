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
    public class AddTripHighlightService : IAddTripHighlightService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHighlightRepository _tripHighlightRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripHighlightDto> _validator;

        public AddTripHighlightService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripHighlightRepository tripHighlightRepository,
            IValidator<CreateTripHighlightDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripHighlightRepository = tripHighlightRepository;
            _validator = validator;
        }

        public async Task<TripHighlightAddedResponseDto> AddTripHighlightAsync(
            Guid tripId,
            CreateTripHighlightDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var highlight = new TripHighlight
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                DisplayOrder = dto.DisplayOrder
            };

            highlight.Translations.Add(new TripHighlightTranslation
            {
                Id = Guid.NewGuid(),
                TripHighlightId = highlight.Id,
                Language = Language.English,
                Description = dto.Description.English?.Trim() ?? string.Empty
            });

            if (!string.IsNullOrWhiteSpace(dto.Description.German))
            {
                highlight.Translations.Add(new TripHighlightTranslation
                {
                    Id = Guid.NewGuid(),
                    TripHighlightId = highlight.Id,
                    Language = Language.German,
                    Description = dto.Description.German.Trim()
                });
            }

            _tripHighlightRepository.Add(highlight);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var englishTranslation = highlight.Translations.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? string.Empty;

            return new TripHighlightAddedResponseDto(
                highlight.Id,
                highlight.TripId,
                description);
        }
    }
}