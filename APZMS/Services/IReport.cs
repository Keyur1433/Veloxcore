using APZMS.DTOs;

namespace APZMS.Services
{
    public interface IReport
    {
        Task<IEnumerable<ActivityRevenueResponse>> GetActivityRevenueAsync(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);
    }
}
