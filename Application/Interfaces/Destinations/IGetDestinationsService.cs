using Application.DTOs.Common;
using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface IGetDestinationsService
    {
        Task<PagedResult<DestinationSummaryDto>> GetDestinationsAsync(
            GetDestinationsQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
