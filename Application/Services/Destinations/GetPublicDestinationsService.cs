using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Destinations
{
    public class GetPublicDestinationsService : IGetPublicDestinationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicDestinationsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PublicDestinationDto>> GetPublicDestinationsAsync(CancellationToken cancellationToken = default)
        {
            var destinations = await _unitOfWork.Destinations.GetActiveAsync(cancellationToken);

            return destinations.Select(d => new PublicDestinationDto(
                d.Id,
                d.Name
            )).ToList();
        }
    }
}
