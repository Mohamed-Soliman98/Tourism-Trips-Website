using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface IGetPublicDestinationsService
    {
        Task<List<PublicDestinationDto>> GetPublicDestinationsAsync(CancellationToken cancellationToken = default);
    }
}
