using APZMS.Domain.Models;
using APZMS.Domain.Models.Views;
using Microsoft.EntityFrameworkCore;

namespace APZMS.Infrastructure.Database
{
    internal class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ActivityGames> Activities { get; set; } 
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingListItem> BookingListItems { get; set; }
    }
}
