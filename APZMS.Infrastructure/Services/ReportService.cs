using APZMS.Application.Common;
using APZMS.Application.DTOs;
using APZMS.Application.Interfaces;
using APZMS.Domain.Exceptions;
using APZMS.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly DynamicPricing _dynamicPricing;
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IBookingRepository _bookingRepository;

        public ReportService(DynamicPricing dynamicPricing, IReportRepository reportRepository, IUserRepository userRepository, IActivityRepository activityRepository, IBookingRepository bookingRepository)
        {
            _dynamicPricing = dynamicPricing;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _bookingRepository = bookingRepository;
            _activityRepository = activityRepository;
        }

        public async Task<IEnumerable<ActivityRevenueResponse>> GetActivityRevenueAsync(string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            var activities = _reportRepository.GetActivityAsQueryable();

            if (!string.IsNullOrWhiteSpace(safetyLevel))
            {
                activities = activities.Where(a => a.SafetyLevel == safetyLevel);
            }

            var bookings = _bookingRepository.GetBookingAsQueryable();

            if (bookingDateFrom.HasValue || bookingDateTo.HasValue)
            {
                if (!bookingDateFrom.HasValue || !bookingDateTo.HasValue)
                {
                    throw new InvalidDateRangeException("Both bookingDateFrom and bookingDateTo must be provided for date filtering.");
                }

                bookings = bookings.Where(b => b.BookingDate >= bookingDateFrom && b.BookingDate <= bookingDateTo);
            }

            //Read `Activity Group By Logic Explain.md` file for explanation of below LINQ query code
            var bookingRows = await (
                from b in bookings
                join a in activities on b.ActivityId equals a.Id
                join u in _userRepository.GetUsers() on b.CustomerId equals u.Id
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

        public async Task<Root> GetCustomerHistoryAsync(string? id, string? role, string? activityName, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {

            var customers = _userRepository.GetUserAsQueryable();
            var bookings = _bookingRepository.GetBookingAsQueryable();
            var activities = _activityRepository.GetActivityAsQueryable();

            int idFromToken = 0;

            if (role == "customer" && !string.IsNullOrWhiteSpace(id))
            {
                idFromToken = int.Parse(id);
                customers = customers.Where(c => c.Id == idFromToken);

                var query = from b in bookings
                            join c in customers on b.CustomerId equals c.Id
                            join a in activities on b.ActivityId equals a.Id
                            where c.Id == idFromToken
                            select new
                            {
                                bookingId = b.BookingId,
                                bookingDate = b.BookingDate,
                                participants = b.Participants,
                                timeSlot = b.TimeSlot,
                                customerId = c.Id,
                                customerName = c.Name,
                                customerDOB = c.BirthDate,
                                activityId = a.Id,
                                activityName = a.Name,
                                slotPrice = a.Price,
                            };

                var bookingRows = await query.ToListAsync();

                var rowsWithPrice = bookingRows.Select(b => new
                {
                    b.bookingId,
                    b.activityId,
                    b.activityName,
                    b.bookingDate,
                    b.timeSlot,
                    b.participants,
                    FinalPrice = _dynamicPricing.GetFinalPrice(b.customerDOB, b.timeSlot, b.slotPrice),
                    b.customerId,
                    b.customerName,
                    b.customerDOB
                }).ToList();

                var totalBookings = rowsWithPrice.Count;
                var totalSpent = rowsWithPrice.Sum(r => r.FinalPrice);

                var favouriteActivityGroup = rowsWithPrice
                    .GroupBy(r => new { r.activityId, r.activityName })
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                var favoriteActivityName = favouriteActivityGroup?.Key.activityName ?? string.Empty;

                var bookingHistory = rowsWithPrice.Select(r => new CustomerHistoryResponse
                {
                    BookingId = r.bookingId,
                    ActivityName = r.activityName,
                    BookingDate = r.bookingDate,
                    TimeSlot = r.timeSlot,
                    Participants = r.participants,
                    FinalPrice = r.FinalPrice,
                }).ToList();

                var result = new Root
                {
                    CustomerId = rowsWithPrice.FirstOrDefault()?.customerId ?? idFromToken,
                    CustomerName = rowsWithPrice.FirstOrDefault()?.customerName ?? string.Empty,
                    AgeGroup = AgeGroupHelper.GetAgeGroup(rowsWithPrice.FirstOrDefault()!.customerDOB),
                    TotalBookings = totalBookings,
                    TotalSpent = totalSpent,
                    FavoriteActivity = favoriteActivityName,
                    BookingHistory = bookingHistory
                };

                return result;
            }
            else
            {
                var query = from b in bookings
                            join c in customers on b.CustomerId equals c.Id
                            join a in activities on b.ActivityId equals a.Id
                            select new
                            {
                                bookingId = b.BookingId,
                                bookingDate = b.BookingDate,
                                participants = b.Participants,
                                timeSlot = b.TimeSlot,
                                customerId = c.Id,
                                customerName = c.Name,
                                customerDOB = c.BirthDate,
                                activityId = a.Id,
                                activityName = a.Name,
                                slotPrice = a.Price,
                            };

                var bookingRows = await query.ToListAsync();

                var rowsWithPrice = bookingRows.Select(b => new
                {
                    b.bookingId,
                    b.activityId,
                    b.activityName,
                    b.bookingDate,
                    b.timeSlot,
                    b.participants,
                    FinalPrice = _dynamicPricing.GetFinalPrice(b.customerDOB, b.timeSlot, b.slotPrice),
                    b.customerId,
                    b.customerName,
                    b.customerDOB
                }).ToList();

                var totalBookings = rowsWithPrice.Count;
                var totalSpent = rowsWithPrice.Sum(r => r.FinalPrice);

                var favouriteActivityGroup = rowsWithPrice
                    .GroupBy(r => new { r.activityId, r.activityName })
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                var favoriteActivityName = favouriteActivityGroup?.Key.activityName ?? string.Empty;

                var bookingHistory = rowsWithPrice.Select(r => new CustomerHistoryResponse
                {
                    BookingId = r.bookingId,
                    ActivityName = r.activityName,
                    BookingDate = r.bookingDate,
                    TimeSlot = r.timeSlot,
                    Participants = r.participants,
                    FinalPrice = r.FinalPrice,
                }).ToList();

                var result = new Root
                {
                    CustomerId = rowsWithPrice.FirstOrDefault()?.customerId ?? idFromToken,
                    CustomerName = rowsWithPrice.FirstOrDefault()?.customerName ?? string.Empty,
                    AgeGroup = AgeGroupHelper.GetAgeGroup(rowsWithPrice.FirstOrDefault()!.customerDOB),
                    TotalBookings = totalBookings,
                    TotalSpent = totalSpent,
                    FavoriteActivity = favoriteActivityName,
                    BookingHistory = bookingHistory
                };

                return result;
            }
        }
    }
}