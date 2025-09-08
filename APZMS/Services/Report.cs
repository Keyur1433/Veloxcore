using APZMS.Data;
using APZMS.DTOs;
using APZMS.Utilities;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Services
{
    public class Report : IReport
    {
        private readonly AppDbContext _context;
        private readonly DynamicPricing _dynamicPricing;

        public Report(AppDbContext context, DynamicPricing dynamicPricing)
        {
            _context = context;
            _dynamicPricing = dynamicPricing;
        }

        public async Task<IEnumerable<ActivityRevenueResponse>> GetActivityRevenueAsync(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            var activities = _context.Activities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(safetyLevel))
            {
                activities = activities.Where(a => a.SafetyLevel == safetyLevel);
            }

            var bookings = _context.Bookings.AsQueryable();

            if (bookingDateFrom.HasValue || bookingDateTo.HasValue)
            {
                if (!bookingDateFrom.HasValue || !bookingDateTo.HasValue)
                {
                    throw new ArgumentException("Both bookingDateFrom and bookingDateTo must be provided for date filtering.");
                }

                bookings = bookings.Where(b => b.BookingDate >= bookingDateFrom && b.BookingDate <= bookingDateTo);
            }

            var bookingRows = await (
                from b in bookings
                join a in _context.Activities on b.ActivityId equals a.Id
                join u in _context.Users on b.CustomerId equals u.Id
                select new
                {
                    ActivityId = a.Id,
                    ActivityName = a.Name!,
                    SafetyLevel = a.SafetyLevel,
                    Participants = b.Participants,
                    SlotPrice = a.Price,
                    TimeSlot = b.TimeSlot,
                    BirthDate = u.BirthDate
                }
            ).ToListAsync();

            var result = bookingRows
                .GroupBy(x => new { x.ActivityId, x.ActivityName, x.SafetyLevel })
                .Select(g =>
                {
                    var totalRevenue = g.Sum(x => _dynamicPricing.GetFinalPrice(x.BirthDate, x.TimeSlot, x.SlotPrice));
                    var totalBookings = g.Count();
                    var totalParticipants = g.Sum(x => x.Participants);

                    return new ActivityRevenueResponse
                    {
                        ActivityId = g.Key.ActivityId,
                        ActivityName = g.Key.ActivityName,
                        SafetyLevel = g.Key.SafetyLevel ?? string.Empty,
                        TotalBookings = totalBookings,
                        TotalParticipants = totalParticipants,
                        TotalRevenue = totalRevenue,
                        AveragePrice = totalRevenue / totalParticipants
                    };
                });
            return result;
        }
    }
}