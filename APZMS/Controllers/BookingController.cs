using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APZMS.Controllers
{
    [ApiController]
    [Route("api/v1/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> GetFilteredBookings(int pageNumber = 1, int pageSize = 20, string? customerName = null, string? activityName = null, string? safetyLevel = null, DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null)
        {
            var result = await _bookingService.GetFilteredBookings(pageNumber, pageSize, customerName, activityName, safetyLevel, bookingDateFrom, bookingDateTo);
            return Ok(result);
        }

        [HttpPost]
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

        [HttpGet("{bookingId}")]
        [Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> GetBookings(string bookingId)
        {
            try
            {
                int id = int.Parse(bookingId);

                var result = await _bookingService.GetBookingsByIdAsync(id);
                return StatusCode(200, new { result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{bookingId}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateBooking(string bookingId, BookingUpdateDto dto)
        {
            try
            {
                int id = int.Parse(bookingId);
                var result = await _bookingService.UpdateBookingAsync(id, dto);

                return StatusCode(200, new { result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{bookingId}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> PatchBooking(string bookingId, BookingPatchDto dto)
        {
            int id = int.Parse(bookingId);
            var result = await _bookingService.PatchBookingAsync(id, dto);

            return StatusCode(200, new { result });
        }

        [HttpDelete("{bookingId}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CancleBooking(string bookingId)
        {
            int id = int.Parse(bookingId);
            var result = await _bookingService.CancleBookingAsync(id);

            return StatusCode(200, new { result });
        }
    }
}