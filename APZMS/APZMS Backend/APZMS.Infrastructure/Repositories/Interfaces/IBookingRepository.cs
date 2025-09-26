using APZMS.Domain.Models;
using APZMS.Domain.Models.Views;

namespace APZMS.Infrastructure.Repositories.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<BookingListItem>> GetFilteredBookings(int? pageNumber, int? pageSize, string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);

        IQueryable<Booking> GetBookingAsQueryable();
    }
}
