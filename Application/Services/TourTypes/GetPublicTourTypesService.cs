using Application.DTOs.TourTypes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class GetPublicTourTypesService : IGetPublicTourTypesService
    {
        private readonly ITourTypeRepository _tourTypeRepository;

        public GetPublicTourTypesService(ITourTypeRepository tourTypeRepository)
        {
            _tourTypeRepository = tourTypeRepository;
        }

        public async Task<List<PublicTourTypeDto>> GetPublicTourTypesAsync(CancellationToken cancellationToken = default)
        {
            var tourTypes = await _tourTypeRepository.GetActiveAsync(cancellationToken);

            return tourTypes.Select(t => new PublicTourTypeDto(
                t.Id,
                t.Name
            )).ToList();
        }
    }
}
