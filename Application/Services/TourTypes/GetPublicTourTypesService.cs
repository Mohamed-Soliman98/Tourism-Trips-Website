using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class GetPublicTourTypesService : IGetPublicTourTypesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicTourTypesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PublicTourTypeDto>> GetPublicTourTypesAsync(CancellationToken cancellationToken = default)
        {
            var tourTypes = await _unitOfWork.TourTypes.GetActiveAsync(cancellationToken);

            return tourTypes.Select(t => new PublicTourTypeDto(
                t.Id,
                t.Name
            )).ToList();
        }
    }
}
