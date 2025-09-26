using APZMS.Application.DTOs;
using APZMS.Domain.Models;
using APZMS.Application.Interfaces;
using APZMS.Application.Common;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using APZMS.Domain.Exceptions;
using APZMS.Infrastructure.Repositories.Interfaces;

namespace APZMS.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly DynamicPricing _dynamicPricing;
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(DynamicPricing dynamicPricing, IMapper mapper, IBookingRepository bookingRepository, IUserRepository userRepository, IActivityRepository activityRepository)
        {
            _dynamicPricing = dynamicPricing;
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _activityRepository = activityRepository;
            _userRepository = userRepository;
        }

        public async Task<BookingResponseDto> AddBookingAsync(BookingDto dto)
        {
            var customer = await _userRepository.GetByIdAsync(dto.CustomerId);
            var activity = await _activityRepository.GetByIdAsync(dto.ActivityId);

            if (customer == null)
                throw new CustomerNotFoundException($"Customer not found with customer id {dto.CustomerId}");

            if (activity == null)
                throw new ActivityNotFoundException($"Activity not found with activity id {dto.ActivityId}");

            if (dto.Participants > activity.Capacity || dto.Participants < 0)
                throw new ArgumentOutOfRangeException("Participants exceed activity capacity.");

            var Booking = new Booking
            {
                CustomerId = dto.CustomerId,
                ActivityId = dto.ActivityId,
                BookingDate = dto.BookingDate,
                TimeSlot = dto.TimeSlot,
                Participants = dto.Participants,
            };

            await _bookingRepository.AddAsync(Booking);
            await _bookingRepository.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = Booking.BookingId,
                ActivityId = dto.ActivityId,
                ActivityName = activity.Name!,
                CustomerId = dto.CustomerId,
                CustomerName = customer.Name!,
                FinalPrice = _dynamicPricing.GetFinalPrice(customer.BirthDate, dto.TimeSlot, activity.Price),
                TimeSlotType = TimeSlotTypeGenerator.GetTimeSlotType(dto.TimeSlot),
                AgeGroup = AgeGroupHelper.GetAgeGroup(customer.BirthDate),
                Message = "Booking successful."
            };
        }

        public async Task<BookingResponseDto> UpdateBookingAsync(int id, BookingUpdateDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
            {
                throw new BookingNotFoundException($"Booking not found with booking id {id}");
            }
            else { }

            var customer = await _userRepository.GetByIdAsync(booking.CustomerId);
            var activity = await _activityRepository.GetByIdAsync(booking.ActivityId);

            if (customer == null)
                throw new CustomerNotFoundException($"Customer not found with customer id {dto.CustomerId}");

            if (activity == null)
                throw new ActivityNotFoundException($"Activity not found with activity id {dto.ActivityId}");

            if (dto.Participants > activity.Capacity || dto.Participants < 0)
                throw new BadHttpRequestException("Participants exceed activity capacity.");

            booking.CustomerId = dto.CustomerId;
            booking.ActivityId = dto.ActivityId;
            booking.BookingDate = dto.BookingDate;
            booking.Participants = dto.Participants;

            await _bookingRepository.SaveChangesAsync();

            return new BookingResponseDto
            {
                BookingId = id,
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

        public async Task<Booking> PatchBookingAsync(int id, BookingPatchDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                throw new BookingNotFoundException($"Booking not found with booking id {id}");

            _mapper.Map(dto, booking);

            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> CancleBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                throw new BookingNotFoundException($"Booking not found with booking id {id}");

            _bookingRepository.Delete(booking);
            await _bookingRepository.SaveChangesAsync();

            return booking;
        }

        public async Task<BookingResponseDto> GetBookingsByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                throw new BookingNotFoundException($"Booking not found with booking id {bookingId}");

            var activity = await _activityRepository.GetByIdAsync(booking.ActivityId);
            var customer = await _userRepository.GetByIdAsync(booking.CustomerId);

            return new BookingResponseDto
            {
                BookingId = bookingId,
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

        public async Task<IEnumerable<BookingFilteredItemResponseDto>> GetFilteredBookings(int? pageNumber, int? pageSize, string? customerName, string? activityName, string? safetyLevel, DateTime? bookingDateFrom, DateTime? bookingDateTo)
        {
            // Validation
            if (pageSize > 500)
            {
                throw new BadHttpRequestException("Maximum page size allowed is 500.");
            }

            if (bookingDateFrom != null && bookingDateTo == null)
            {
                throw new BadHttpRequestException("From date and To date both are must to provide");
            }

            var rows = await _bookingRepository.GetFilteredBookings(pageNumber, pageSize, customerName, activityName, safetyLevel, bookingDateFrom, bookingDateTo);

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