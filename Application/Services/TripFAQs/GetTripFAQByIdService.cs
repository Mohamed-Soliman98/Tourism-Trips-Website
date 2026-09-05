using Application.DTOs.TripFAQs;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripFAQs;

namespace Application.Services.TripFAQs
{
    public class GetTripFAQByIdService : IGetTripFAQByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IFAQRepository _faqRepository;

        public GetTripFAQByIdService(
            ITripRepository tripRepository,
            IFAQRepository faqRepository)
        {
            _tripRepository = tripRepository;
            _faqRepository = faqRepository;
        }

        public async Task<TripFAQResponseDto> GetTripFAQByIdAsync(
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

            return new TripFAQResponseDto(
                faq.Id,
                faq.TripId!.Value,
                faq.Question,
                faq.Answer,
                faq.DisplayOrder,
                faq.IsActive);
        }
    }
}