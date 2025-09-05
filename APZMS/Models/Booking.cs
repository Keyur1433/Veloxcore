namespace APZMS.Models
{
    public class Booking
    {
        public int BookingId {  get; set; }
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = default!;
        public int Participants { get; set; }
    }
}
