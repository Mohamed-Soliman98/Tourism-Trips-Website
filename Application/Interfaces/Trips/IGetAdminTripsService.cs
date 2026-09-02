using Application.DTOs.Common;
using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IGetAdminTripsService
    {
        Task<PagedResult<AdminTripSummaryDto>> GetAdminTripsAsync(GetAdminTripsQueryDto query, CancellationToken cancellationToken = default);
    }
}
