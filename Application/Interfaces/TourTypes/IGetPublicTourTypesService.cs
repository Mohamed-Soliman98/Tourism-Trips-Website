using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface IGetPublicTourTypesService
    {
        Task<List<PublicTourTypeDto>> GetPublicTourTypesAsync(CancellationToken cancellationToken = default);
    }
}
