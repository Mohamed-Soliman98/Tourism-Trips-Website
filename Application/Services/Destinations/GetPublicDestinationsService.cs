using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.Repositories;

namespace Application.Services.Destinations
{
    public class GetPublicDestinationsService : IGetPublicDestinationsService
    {
        private readonly IDestinationRepository _destinationRepository;

        public GetPublicDestinationsService(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        public async Task<List<PublicDestinationDto>> GetPublicDestinationsAsync(CancellationToken cancellationToken = default)
        {
            var destinations = await _destinationRepository.GetActiveAsync(cancellationToken);

            return destinations.Select(d => new PublicDestinationDto(
                d.Id,
                d.Name
            )).ToList();
        }
    }
}
