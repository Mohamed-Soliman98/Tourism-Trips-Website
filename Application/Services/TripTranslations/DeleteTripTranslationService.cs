using Application.DTOs.TripTranslations;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripTranslations;

namespace Application.Services.TripTranslations
{
    public class DeleteTripTranslationService : IDeleteTripTranslationService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripTranslationRepository _tripTranslationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripTranslationService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripTranslationRepository tripTranslationRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripTranslationRepository = tripTranslationRepository;
        }

        public async Task<TripTranslationDeletedResponseDto> DeleteTripTranslationAsync(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (translationId == Guid.Empty)
                throw new ArgumentException("Translation Id cannot be empty.", nameof(translationId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var translation = await _tripTranslationRepository.GetByIdAsync(translationId, cancellationToken);
            if (translation == null)
                throw new KeyNotFoundException($"Trip translation with ID '{translationId}' was not found.");

            if (translation.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip translation '{translationId}' does not belong to trip '{tripId}'.");

            _tripTranslationRepository.Remove(translation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripTranslationDeletedResponseDto();
        }
    }
}
