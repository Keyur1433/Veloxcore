namespace APZMS.Application.DTOs
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = default!;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public decimal FinalPrice { get; set; }
        public string TimeSlotType { get; set; } = default!;
        public string AgeGroup { get; set; } = default!;
        public string Message { get; set; } = "Booking successful.";
    }

}
