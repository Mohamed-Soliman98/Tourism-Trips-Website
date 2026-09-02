using Application.DTOs.Dashboard;
using Application.Interfaces.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IGetDashboardSummaryService _getDashboardSummaryService;

        public DashboardController(IGetDashboardSummaryService getDashboardSummaryService)
        {
            _getDashboardSummaryService = getDashboardSummaryService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<DashboardSummaryDto>> GetSummary(CancellationToken cancellationToken)
        {
            var summary = await _getDashboardSummaryService.GetDashboardSummaryAsync(cancellationToken);
            return Ok(summary);
        }
    }
}
