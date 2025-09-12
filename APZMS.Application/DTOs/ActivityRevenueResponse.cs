namespace APZMS.Application.DTOs
{
    public class ActivityRevenueResponse
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; } = default!;
        public string SafetyLevel { get; set; } = default!;
        public int TotalBookings { get; set; } 
        public int TotalParticipants { get; set; }
        public decimal TotalRevenue {get; set; }
        public decimal AveragePrice {get; set; }
    }
}
