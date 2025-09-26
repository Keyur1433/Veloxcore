namespace APZMS.Application.DTOs
{
    public class CustomerHistoryResponse
    {
        public int BookingId { get; set; }
        public string ActivityName { get; set; } = default!;
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = default!;
        public int Participants { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; } = "completed";
    }

    public class Root
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string AgeGroup { get; set; } = default!;
        public int TotalBookings { get; set; }
        public decimal TotalSpent { get; set; }
        public string FavoriteActivity { get; set; } = default!;
        public List<CustomerHistoryResponse> BookingHistory { get; set; } = default!;
    }
}
