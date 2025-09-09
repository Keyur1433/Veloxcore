using APZMS.DTOs;

namespace APZMS.Services
{
    public interface IReportService
    {
        Task<IEnumerable<ActivityRevenueResponse>> GetActivityRevenueAsync(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);
        Task<Root> GetCustomerHistoryAsync(string? id, string? role, string? activityName, DateTime? bookingDateFrom, DateTime? bookingDateTo);
    }
}
