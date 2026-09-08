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
    public class AddTripWhatToBringService : IAddTripWhatToBringService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripWhatToBringRepository _tripWhatToBringRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripWhatToBringDto> _validator;

        public AddTripWhatToBringService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripWhatToBringRepository tripWhatToBringRepository,
            IValidator<CreateTripWhatToBringDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripWhatToBringRepository = tripWhatToBringRepository;
            _validator = validator;
        }

        public async Task<TripWhatToBringAddedResponseDto> AddTripWhatToBringAsync(
            Guid tripId,
            CreateTripWhatToBringDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var whatToBring = new TripWhatToBring
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                DisplayOrder = dto.DisplayOrder
            };

            whatToBring.Translations.Add(new TripWhatToBringTranslation
            {
                Id = Guid.NewGuid(),
                TripWhatToBringId = whatToBring.Id,
                Language = Language.English,
                Description = dto.Description.English?.Trim() ?? string.Empty
            });

            if (!string.IsNullOrWhiteSpace(dto.Description.German))
            {
                whatToBring.Translations.Add(new TripWhatToBringTranslation
                {
                    Id = Guid.NewGuid(),
                    TripWhatToBringId = whatToBring.Id,
                    Language = Language.German,
                    Description = dto.Description.German.Trim()
                });
            }

            _tripWhatToBringRepository.Add(whatToBring);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var englishTranslation = whatToBring.Translations.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? string.Empty;

            return new TripWhatToBringAddedResponseDto(
                whatToBring.Id,
                whatToBring.TripId,
                description);
        }
    }
}