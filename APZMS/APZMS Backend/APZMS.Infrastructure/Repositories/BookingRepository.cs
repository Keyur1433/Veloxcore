using APZMS.Infrastructure.Database;
using APZMS.Domain.Models;
using APZMS.Domain.Models.Views;
using Microsoft.EntityFrameworkCore;
using APZMS.Infrastructure.Repositories.Interfaces;

namespace APZMS.Infrastructure.Repositories
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) { _context = context; }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.AddAsync(booking);
            return booking;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public Booking Delete(Booking item)
        {
            _context.Bookings.Remove(item);
            return item;
        }

        public async Task<IEnumerable<BookingListItem>> GetFilteredBookings(int? pageNumber, int? pageSize, string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            var rows = await _context.Set<BookingListItem>()
                .FromSqlInterpolated($"EXEC sp_GetBookings @PageNumber={pageNumber}, @PageSize={pageSize}, @CustomerName={customerName}, @ActivityName={activityName}, @SafetyLevel={safetyLevel}, @BookingDateFrom={bookingDateFrom}, @BookingDateTo={bookingDateTo}")
                .ToListAsync();

            return rows;
        }

        public IQueryable<Booking> GetBookingAsQueryable()
        {
            return _context.Bookings.AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
