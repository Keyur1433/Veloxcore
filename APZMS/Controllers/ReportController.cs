using APZMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _report;

        public ReportController(IReportService report)
        {
            _report = report;
        }

        [HttpGet("activity-revenue")]
        [Authorize(Roles = "admin, staff")]
        public async Task<IActionResult> GetActivityRevenue([FromQuery]string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            try
            {
                var result = await _report.GetActivityRevenueAsync(safetyLevel, bookingDateFrom, bookingDateTo);

                return Ok(new { result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer-history/{customerId}")]
        [Authorize(Roles = "admin, staff, customer")]
        public async Task<IActionResult> GetCustomerHistory(string? customerId, string? activityName, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            string role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(customerId) && (role == "admin" || role == "staff"))
            {
                id = customerId;
            }
            else
            {
                return BadRequest("customerId not provided");
            }

                var result = await _report.GetCustomerHistoryAsync(id, role, activityName, bookingDateFrom, bookingDateTo);

            return Ok(new { result });  
        }
    }
}