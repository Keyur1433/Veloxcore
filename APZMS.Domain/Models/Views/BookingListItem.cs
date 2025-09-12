using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace APZMS.Domain.Models.Views
{
// The[Keyless] attribute in Entity Framework Core(EF Core) indicates that the class is not a regular entity type with a primary key, which means EF Core should not expect a unique key(typically marked by[Key] attribute) or track it as a standard entity with identity.

//Meaning of[Keyless]:
//No Primary Key: The class does not have a primary key, which is usually used to uniquely identify records in a database table.

// When to Use[Keyless]:
//Query Types: You use it when your class is meant to be used in EF Core queries but does not represent a table that you intend to modify.For example, when you're reading from a view or a stored procedure that returns a result but doesn�t have a primary key.

//Complex Queries: If you're returning complex or join results from raw SQL queries, and you don't need to modify the data, marking the class as Keyless ensures EF Core doesn't expect it to be tracked.

    [Keyless]
    public class BookingListItem
    {
        public int BookingId { get; set; }
        public int ActivityId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string ActivityName { get; set; } = default!;
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = default!;
        public int Participants { get; set; }
        public DateTime CustomerBirthDate { get; set; }
    }
}

