namespace APZMS.Application.DTOs
{
    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string? SafetyLevel { get; set; }
        public string? PhotoUrl { get; set; } 
    }
}
