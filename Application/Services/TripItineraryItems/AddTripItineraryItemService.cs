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
    public class AddTripItineraryItemService : IAddTripItineraryItemService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripItineraryItemDto> _validator;

        public AddTripItineraryItemService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripItineraryItemRepository tripItineraryItemRepository,
            IValidator<CreateTripItineraryItemDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
            _validator = validator;
        }

        public async Task<TripItineraryItemAddedResponseDto> AddTripItineraryItemAsync(
            Guid tripId,
            CreateTripItineraryItemDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var itineraryItem = new TripItineraryItem
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                DisplayOrder = dto.DisplayOrder
            };

            itineraryItem.Translations.Add(new TripItineraryItemTranslation
            {
                Id = Guid.NewGuid(),
                TripItineraryItemId = itineraryItem.Id,
                Language = Language.English,
                Title = dto.Title.English?.Trim() ?? string.Empty,
                Description = dto.Description?.English?.Trim()
            });

            if (!string.IsNullOrWhiteSpace(dto.Title.German) || !string.IsNullOrWhiteSpace(dto.Description?.German))
            {
                itineraryItem.Translations.Add(new TripItineraryItemTranslation
                {
                    Id = Guid.NewGuid(),
                    TripItineraryItemId = itineraryItem.Id,
                    Language = Language.German,
                    Title = string.IsNullOrWhiteSpace(dto.Title.German) ? (dto.Title.English?.Trim() ?? string.Empty) : dto.Title.German.Trim(),
                    Description = string.IsNullOrWhiteSpace(dto.Description?.German) ? dto.Description?.English?.Trim() : dto.Description.German.Trim()
                });
            }

            _tripItineraryItemRepository.Add(itineraryItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var englishTranslation = itineraryItem.Translations.FirstOrDefault(t => t.Language == Language.English);

            return new TripItineraryItemAddedResponseDto(
                itineraryItem.Id,
                itineraryItem.TripId,
                itineraryItem.DisplayOrder,
                englishTranslation?.Title ?? string.Empty,
                englishTranslation?.Description);
        }
    }
}