using System.ComponentModel.DataAnnotations.Schema;

namespace APZMS.Domain.Models
{
    public class ActivityGames
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string? SafetyLevel { get; set; }
        public string? PhotoUrl { get; set; } // file path stored in DB

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

