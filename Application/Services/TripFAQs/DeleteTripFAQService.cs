using Application.DTOs.TripFAQs;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripFAQs;

namespace Application.Services.TripFAQs
{
    public class DeleteTripFAQService : IDeleteTripFAQService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IFAQRepository _faqRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripFAQService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            IFAQRepository faqRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _faqRepository = faqRepository;
        }

        public async Task<TripFAQDeletedResponseDto> DeleteTripFAQAsync(
            Guid tripId,
            Guid faqId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (faqId == Guid.Empty)
                throw new ArgumentException("FAQ Id cannot be empty.", nameof(faqId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var faq = await _faqRepository.GetByIdAsync(faqId, cancellationToken);
            if (faq == null)
                throw new KeyNotFoundException($"Trip FAQ with ID '{faqId}' was not found.");

            if (faq.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip FAQ '{faqId}' does not belong to trip '{tripId}'.");

            _faqRepository.Remove(faq);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripFAQDeletedResponseDto(faq.Id);
        }
    }
}