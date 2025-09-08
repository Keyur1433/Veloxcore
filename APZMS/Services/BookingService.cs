using APZMS.Data;
using APZMS.DTOs;
using APZMS.Models;
using APZMS.Models.Views;
using APZMS.Utilities;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly DynamicPricing _dynamicPricing;

        public BookingService(AppDbContext context, DynamicPricing dynamicPricing)
        {
            _context = context;
            _dynamicPricing = dynamicPricing;
        }

        public async Task<BookingResponseDto> AddBookingAsync(BookingDto dto)
        {
            var customer = await _context.Users.FindAsync(dto.CustomerId);
            var activity = await _context.Activities.FindAsync(dto.ActivityId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            if (activity == null)
                throw new KeyNotFoundException("Activity not found");

            if (dto.Participants > activity.Capacity && dto.Participants < 0)
                throw new ArgumentOutOfRangeException("Participants exceed activity capacity.");

            var Booking = new Booking
            {
                CustomerId = dto.CustomerId,
                ActivityId = dto.ActivityId,
                BookingDate = dto.BookingDate,
                TimeSlot = dto.TimeSlot,
                Participants = dto.Participants,
            };

            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = Booking.BookingId,
                ActivityId = dto.ActivityId,
                ActivityName = activity.Name!,
                CustomerId = dto.CustomerId,
                CustomerName = customer.Name!,
                FinalPrice = _dynamicPricing.GetFinalPrice(customer.BirthDate, dto.TimeSlot, activity.Price),
                TimeSlotType = TimeSlotTypeGenerator.GetTimeSlotType(dto.TimeSlot),
                Message = "Booking successful."
            };
        }

        public async Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            if (bookingDateFrom != null)
            {
                if (bookingDateTo == null )
                    throw new ArgumentException("From date and To date both are must to provide");
            }

            var rows = await _context.Set<BookingListItem>()
                .FromSqlInterpolated($"EXEC sp_GetBookings @CustomerName={customerName}, @ActivityName={activityName}, @SafetyLevel={safetyLevel}, @BookingDateFrom={bookingDateFrom}, @BookingDateTo={bookingDateTo}")
                .ToListAsync();

            return rows.Select(r => new BookingFilteredItemResponseDto
            {
                Id = r.BookingId,
                ActivityId = r.ActivityId,
                CustomerId = r.CustomerId,
                CustomerName = r.CustomerName,
                ActivityName = r.ActivityName,
                Price = r.Price,
                FinalPrice = _dynamicPricing.GetFinalPrice(r.CustomerBirthDate, r.TimeSlot, r.Price),
                BookingDate = r.BookingDate,
                TimeSlot = r.TimeSlot,
                Participants = r.Participants,
                TimeSlotType = TimeSlotTypeGenerator.GetTimeSlotType(r.TimeSlot)
            });
        }
    }
}