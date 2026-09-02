using Application.DTOs.TripFAQs;

namespace Application.Interfaces.TripFAQs
{
    public interface IDeleteTripFAQService
    {
        Task<TripFAQDeletedResponseDto> DeleteTripFAQAsync(
            Guid tripId,
            Guid faqId,
            CancellationToken cancellationToken = default);
    }
}