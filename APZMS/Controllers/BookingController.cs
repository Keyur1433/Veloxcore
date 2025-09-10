using APZMS.DTOs;
using APZMS.Services;
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

        // ToDo:
        // (Follow rest api conventions)
        // Get all bookings with filters and pagination
        // Get by id
        // Post
        // Put
        // Patch
        // Delete

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
                var result = await _bookingService.GetBookingsAsync(bookingId);
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

        [HttpPut("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateBooking(string id, BookingUpdateDto dto)
        {
            try
            {
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

        [HttpPatch("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> PatchCustomer(string id, BookingPatchDto dto)
        {
            var result = await _bookingService.PatchBookingAsync(id, dto);

            return StatusCode(200, new { result });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CancleBooking(string id)
        {
            var result = await _bookingService.CancleBookingAsync(id);

            return StatusCode(200, new { result });
        }

        [HttpGet("filter")]
        [Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> GetFilteredBookings([FromQuery] string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            try
            {
                var filteredBookings = await _bookingService.GetFilteredBookings(customerName, activityName, safetyLevel, bookingDateFrom, bookingDateTo);

                return Ok(new { filteredBookings });
            }
            catch (ArgumentException ex)
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