using APZMS.Data;
using APZMS.DTOs;
using APZMS.Migrations;
using APZMS.Models;
using APZMS.Models.Views;
using APZMS.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace APZMS.Services
{
    public class BookingService : IBookingService
    {
        // Todo: Dont use DBContext directly. Create repository layer to access DB context
        private readonly AppDbContext _context;
        private readonly DynamicPricing _dynamicPricing;
        private readonly IMapper _mapper;

        public BookingService(AppDbContext context, DynamicPricing dynamicPricing, IMapper mapper)
        {
            _context = context;
            _dynamicPricing = dynamicPricing;
            _mapper = mapper;
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

        public async Task<BookingResponseDto> UpdateBookingAsync(string id, BookingUpdateDto dto)
        {
            int bookingId = int.Parse(id);

            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.BookingId == bookingId);
            var customer = await _context.Users.SingleOrDefaultAsync(u => u.Id == dto.CustomerId);
            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.Id == dto.ActivityId);

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            if (activity == null)
                throw new KeyNotFoundException("Activity not found");

            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            booking.CustomerId = dto.CustomerId;
            booking.ActivityId = dto.ActivityId;
            booking.BookingDate = dto.BookingDate;
            booking.Participants = dto.Participants;

            await _context.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = bookingId,
                ActivityId = dto.ActivityId,
                ActivityName = activity!.Name!,
                CustomerId = dto.CustomerId,
                CustomerName = customer!.Name!,
                FinalPrice = _dynamicPricing.GetFinalPrice(customer.BirthDate, booking.TimeSlot, activity.Price),
                TimeSlotType = TimeSlotTypeGenerator.GetTimeSlotType(booking.TimeSlot),
                AgeGroup = AgeGroupHelper.GetAgeGroup(customer.BirthDate),
                Message = "Booking updated."
            };
        }

        public async Task<Booking> PatchBookingAsync(string id, BookingPatchDto dto)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.BookingId == int.Parse(id));

            if (booking == null)
                throw new KeyNotFoundException($"{nameof(booking)} not found");

            _mapper.Map(dto, booking);

            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> CancleBookingAsync(string id)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.BookingId == int.Parse(id));

            if (booking == null)
                throw new KeyNotFoundException($"{nameof(booking)} not found");

            _context.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<BookingResponseDto> GetBookingsAsync(string bookingId)
        {
            var booking = await _context.Bookings.SingleOrDefaultAsync(b => b.BookingId == int.Parse(bookingId));

            if (booking == null)
                throw new KeyNotFoundException("Booking not found");

            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.Id == int.Parse(bookingId));
            var customer = await _context.Users.SingleOrDefaultAsync(u => u.Id == int.Parse(bookingId));

            return new BookingResponseDto
            {
                BookingId = int.Parse(bookingId),
                ActivityId = activity!.Id,
                ActivityName = activity!.Name!,
                CustomerId = customer!.Id,
                CustomerName = customer!.Name!,
                FinalPrice = _dynamicPricing.GetFinalPrice(customer.BirthDate, booking.TimeSlot, activity.Price),
                TimeSlotType = TimeSlotTypeGenerator.GetTimeSlotType(booking.TimeSlot),
                AgeGroup = AgeGroupHelper.GetAgeGroup(customer.BirthDate),
                Message = "Booking found."
            };
        }

        public async Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            if (bookingDateFrom != null)
            {
                if (bookingDateTo == null)
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