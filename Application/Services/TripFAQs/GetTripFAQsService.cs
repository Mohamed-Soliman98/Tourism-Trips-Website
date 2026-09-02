using Application.DTOs.TripFAQs;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripFAQs;

namespace Application.Services.TripFAQs
{
    public class GetTripFAQsService : IGetTripFAQsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripFAQsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TripFAQResponseDto>> GetTripFAQsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var faqs = await _unitOfWork.FAQs.GetByTripIdAsync(tripId, cancellationToken);

            return faqs.Select(f => new TripFAQResponseDto(
                f.Id,
                f.TripId!.Value,
                f.Question,
                f.Answer,
                f.DisplayOrder,
                f.IsActive)).ToList();
        }
    }
}