using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface IDeleteDestinationService
    {
        Task<DestinationDeletedResponseDto> DeleteDestinationAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
