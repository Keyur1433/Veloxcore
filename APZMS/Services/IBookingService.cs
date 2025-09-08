using APZMS.DTOs;

namespace APZMS.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> AddBookingAsync(BookingDto dto);
        Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);
    }
}
