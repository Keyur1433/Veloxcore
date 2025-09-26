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
        public async Task<IActionResult> GetFilteredBookings(int? pageNumber = null, int? pageSize = null, string? customerName = null, string? activityName = null, string? safetyLevel = null, DateTime? bookingDateFrom = null, DateTime? bookingDateTo = null)
        {
            var filteredBookings = await _bookingService.GetFilteredBookings(pageNumber, pageSize, customerName, activityName, safetyLevel, bookingDateFrom, bookingDateTo);

            return Ok(filteredBookings);
        }

        [HttpPost]
        public async Task<IActionResult> BookSlots(BookingDto dto)
        {
            var booking = await _bookingService.AddBookingAsync(dto);
            return Ok(booking);
        }

        [HttpGet("{bookingId}")]
        [Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> GetBooking(int bookingId)
        {
            var result = await _bookingService.GetBookingsByIdAsync(bookingId);
            return Ok(result);
        }

        [HttpPut("{bookingId}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateBooking(int bookingId, BookingUpdateDto dto)
        {
            var result = await _bookingService.UpdateBookingAsync(bookingId, dto);
            return Ok(result);
        }

        [HttpPatch("{bookingId}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> PatchBooking(int bookingId, BookingPatchDto dto)
        {
            var result = await _bookingService.PatchBookingAsync(bookingId, dto);
            return Ok(result);
        }

        [HttpDelete("{bookingId}")]
        [Authorize(Roles = "customer, staff")]
        public async Task<IActionResult> CancleBooking(int bookingId)
        {
            var result = await _bookingService.CancleBookingAsync(bookingId);
            return Ok(result);
        }
    }
}