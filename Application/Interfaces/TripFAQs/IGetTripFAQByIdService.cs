using Application.DTOs.TripFAQs;

namespace Application.Interfaces.TripFAQs
{
    public interface IGetTripFAQByIdService
    {
        Task<TripFAQResponseDto> GetTripFAQByIdAsync(
            Guid tripId,
            Guid faqId,
            CancellationToken cancellationToken = default);
    }
}