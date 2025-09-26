using APZMS.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace APZMS.Application.DTOs
{
    public class BookingDto
    {
        [Required(ErrorMessage = "Valid customer ID is required.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Valid activity ID is required.")]
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Booking date is required.")]
        [FutureDate]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Valid time slot is required (HH:mm).")]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Valid time slot is required (HH:mm).")]
        public string TimeSlot { get; set; } = default!;

        [Range(1, int.MaxValue, ErrorMessage = "Participants must be greater than 0.")]
        public int Participants { get; set; }
    }
}
