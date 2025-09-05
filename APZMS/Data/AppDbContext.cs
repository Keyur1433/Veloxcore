using APZMS.Models;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActivityGames> Activities { get; set; } 
        public DbSet<Booking> Bookings { get; set; }
    }
}
