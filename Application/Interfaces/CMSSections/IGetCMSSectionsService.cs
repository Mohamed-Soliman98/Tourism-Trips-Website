using Application.DTOs.CMSSections;
using Application.DTOs.Common;

namespace Application.Interfaces.CMSSections
{
    public interface IGetCMSSectionsService
    {
        Task<PagedResult<CMSSectionSummaryDto>> GetCMSSectionsAsync(GetCMSSectionsQueryDto query, CancellationToken cancellationToken);
    }
}