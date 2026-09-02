using Application.DTOs.TripFAQs;

namespace Application.Interfaces.TripFAQs
{
    public interface IGetTripFAQsService
    {
        Task<List<TripFAQResponseDto>> GetTripFAQsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}