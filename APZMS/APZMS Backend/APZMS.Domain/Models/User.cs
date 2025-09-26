namespace APZMS.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } = "customer";
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdatedBy { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
