using APZMS.Application.DTOs;
using APZMS.Domain.Models;

namespace APZMS.Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> AddBookingAsync(BookingDto dto);

        Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(int? pageNumber, int? pageSize, string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo);

        Task<BookingResponseDto> GetBookingsByIdAsync(int bookingId);

        Task<BookingResponseDto> UpdateBookingAsync(int id, BookingUpdateDto dto);

        Task<Booking> PatchBookingAsync(int id, BookingPatchDto dto);

        Task<Booking> CancleBookingAsync(int id);
    }
}
