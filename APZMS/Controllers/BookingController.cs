using APZMS.DTOs;
using APZMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("bookings")]
        public async Task<IActionResult> BookSlots(BookingDto dto)
        {
            try
            {
                var booking = await _bookingService.AddBookingAsync(dto);
                return Ok(new { booking });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }
    }
}
