namespace APZMS.Application.DTOs
{
    public class BookingUpdateDto
    {
        public int CustomerId { get; set; }
        public int ActivityId { get; set; }
        public DateTime BookingDate { get; set; }
        public int Participants {  get; set; }
    }
}
