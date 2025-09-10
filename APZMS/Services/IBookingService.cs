using APZMS.DTOs;
using APZMS.Models;

namespace APZMS.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> AddBookingAsync(BookingDto dto);

        Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);

        Task<BookingResponseDto> GetBookingsAsync(string bookingId);

        Task<BookingResponseDto> UpdateBookingAsync(string id, BookingUpdateDto dto);

        Task<Booking> PatchBookingAsync(string id, BookingPatchDto dto);

        Task<Booking> CancleBookingAsync(string id);
    }
}
