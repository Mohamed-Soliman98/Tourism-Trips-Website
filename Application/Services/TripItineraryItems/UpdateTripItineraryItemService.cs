using Application.DTOs.TripItineraryItems;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripItineraryItems;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.TripItineraryItems
{
    public class UpdateTripItineraryItemService : IUpdateTripItineraryItemService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripItineraryItemDto> _validator;

        public UpdateTripItineraryItemService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripItineraryItemRepository tripItineraryItemRepository,
            IValidator<UpdateTripItineraryItemDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
            _validator = validator;
        }

        public async Task<TripItineraryItemUpdatedResponseDto> UpdateTripItineraryItemAsync(
            Guid tripId,
            Guid itineraryItemId,
            UpdateTripItineraryItemDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (itineraryItemId == Guid.Empty)
                throw new ArgumentException("Itinerary item Id cannot be empty.", nameof(itineraryItemId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var item = await _tripItineraryItemRepository.GetByIdWithTranslationsAsync(itineraryItemId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip itinerary item with ID '{itineraryItemId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip itinerary item '{itineraryItemId}' does not belong to trip '{tripId}'.");

            item.DisplayOrder = dto.DisplayOrder;

            var englishTitle = dto.Title.English?.Trim() ?? string.Empty;
            var englishDescription = dto.Description?.English?.Trim();
            var germanTitle = dto.Title.German?.Trim();
            var germanDescription = dto.Description?.German?.Trim();

            var englishTranslation = item.Translations.FirstOrDefault(t => t.Language == Language.English);
            if (englishTranslation == null)
            {
                item.Translations.Add(new TripItineraryItemTranslation
                {
                    Id = Guid.NewGuid(),
                    TripItineraryItemId = item.Id,
                    Language = Language.English,
                    Title = englishTitle,
                    Description = englishDescription
                });
            }
            else
            {
                englishTranslation.Title = englishTitle;
                englishTranslation.Description = englishDescription;
            }

            var germanTranslation = item.Translations.FirstOrDefault(t => t.Language == Language.German);
            if (string.IsNullOrWhiteSpace(germanTitle) && string.IsNullOrWhiteSpace(germanDescription))
            {
                if (germanTranslation != null)
                    item.Translations.Remove(germanTranslation);
            }
            else if (germanTranslation == null)
            {
                item.Translations.Add(new TripItineraryItemTranslation
                {
                    Id = Guid.NewGuid(),
                    TripItineraryItemId = item.Id,
                    Language = Language.German,
                    Title = string.IsNullOrWhiteSpace(germanTitle) ? englishTitle : germanTitle,
                    Description = string.IsNullOrWhiteSpace(germanDescription) ? englishDescription : germanDescription
                });
            }
            else
            {
                germanTranslation.Title = string.IsNullOrWhiteSpace(germanTitle) ? englishTitle : germanTitle;
                germanTranslation.Description = string.IsNullOrWhiteSpace(germanDescription) ? englishDescription : germanDescription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripItineraryItemUpdatedResponseDto(
                item.Id,
                item.TripId,
                item.DisplayOrder,
                englishTitle,
                englishDescription);
        }
    }
}