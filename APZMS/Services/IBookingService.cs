using APZMS.DTOs;

namespace APZMS.Services
{
    public interface IBookingService
    {
        Task<BookingResponseDto> AddBookingAsync(BookingDto dto);
    }
}
