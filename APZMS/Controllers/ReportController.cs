using APZMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReport _report;

        public ReportController(IReport report)
        {
            _report = report;
        }

        [HttpPost("activity-revenue")]
        public async Task<IActionResult> GetActivityRevenue(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            var result = _report.GetActivityRevenueAsync(safetyLevel, bookingDateFrom, bookingDateTo);

            return Ok(result);
        }
    }
}
