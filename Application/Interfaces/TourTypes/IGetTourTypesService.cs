using Application.DTOs.Common;
using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface IGetTourTypesService
    {
        Task<PagedResult<TourTypeSummaryDto>> GetTourTypesAsync(
            GetTourTypesQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
