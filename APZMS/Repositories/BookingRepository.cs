using APZMS.Data;
using APZMS.DTOs;
using APZMS.Models;

namespace APZMS.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) { _context = context; }

        public async Task<BookingResponseDto> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        //public async Task<IEnumerable<Booking>> IRepository<Booking>.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<Booking?> IBookingRepository.GetBookingByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        Task<Booking?> IRepository<Booking>.GetByIdAsync(int id)
        {
            
        }

        Task<int> IRepository<Booking>.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
