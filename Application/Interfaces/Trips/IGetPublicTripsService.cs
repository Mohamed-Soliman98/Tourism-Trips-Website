using Application.DTOs.Common;
using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IGetPublicTripsService
    {
        Task<PagedResult<PublicTripSummaryDto>> GetPublicTripsAsync(GetPublicTripsQueryDto query, CancellationToken cancellationToken = default);
    }
}
