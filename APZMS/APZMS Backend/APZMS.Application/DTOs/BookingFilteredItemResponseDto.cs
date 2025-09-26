namespace APZMS.Application.DTOs
{
    public class BookingFilteredItemResponseDto
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string ActivityName { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = default!;
        public int Participants { get; set; }
        public string TimeSlotType { get; set; } = default!;
    }
}


