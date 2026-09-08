using Application.DTOs.TripWhatToBrings;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripWhatToBrings;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.TripWhatToBrings
{
    public class UpdateTripWhatToBringService : IUpdateTripWhatToBringService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripWhatToBringRepository _tripWhatToBringRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripWhatToBringDto> _validator;

        public UpdateTripWhatToBringService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripWhatToBringRepository tripWhatToBringRepository,
            IValidator<UpdateTripWhatToBringDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripWhatToBringRepository = tripWhatToBringRepository;
            _validator = validator;
        }

        public async Task<TripWhatToBringUpdatedResponseDto> UpdateTripWhatToBringAsync(
            Guid tripId,
            Guid whatToBringId,
            UpdateTripWhatToBringDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (whatToBringId == Guid.Empty)
                throw new ArgumentException("What-to-bring Id cannot be empty.", nameof(whatToBringId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var item = await _tripWhatToBringRepository.GetByIdWithTranslationsAsync(whatToBringId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip what-to-bring item with ID '{whatToBringId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip what-to-bring item '{whatToBringId}' does not belong to trip '{tripId}'.");

            item.DisplayOrder = dto.DisplayOrder;

            var englishDescription = dto.Description.English?.Trim() ?? string.Empty;
            var germanDescription = dto.Description.German?.Trim();

            var englishTranslation = item.Translations.FirstOrDefault(t => t.Language == Language.English);
            if (englishTranslation == null)
            {
                item.Translations.Add(new TripWhatToBringTranslation
                {
                    Id = Guid.NewGuid(),
                    TripWhatToBringId = item.Id,
                    Language = Language.English,
                    Description = englishDescription
                });
            }
            else
            {
                englishTranslation.Description = englishDescription;
            }

            var germanTranslation = item.Translations.FirstOrDefault(t => t.Language == Language.German);
            if (string.IsNullOrWhiteSpace(germanDescription))
            {
                if (germanTranslation != null)
                    item.Translations.Remove(germanTranslation);
            }
            else if (germanTranslation == null)
            {
                item.Translations.Add(new TripWhatToBringTranslation
                {
                    Id = Guid.NewGuid(),
                    TripWhatToBringId = item.Id,
                    Language = Language.German,
                    Description = germanDescription
                });
            }
            else
            {
                germanTranslation.Description = germanDescription;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripWhatToBringUpdatedResponseDto(
                item.Id,
                item.TripId,
                englishDescription);
        }
    }
}