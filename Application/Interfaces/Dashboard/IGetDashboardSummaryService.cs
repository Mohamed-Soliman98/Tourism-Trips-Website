using Application.DTOs.Dashboard;

namespace Application.Interfaces.Dashboard
{
    public interface IGetDashboardSummaryService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    }
}
