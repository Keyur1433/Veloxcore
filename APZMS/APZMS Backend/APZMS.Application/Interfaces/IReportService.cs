using APZMS.Application.DTOs;

namespace APZMS.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ActivityRevenueResponse>> GetActivityRevenueAsync(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);
        Task<Root> GetCustomerHistoryAsync(string? id, string? role, string? activityName, DateTime? bookingDateFrom, DateTime? bookingDateTo);
    }
}
