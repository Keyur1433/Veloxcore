using System.ComponentModel.DataAnnotations;

namespace APZMS.Application.Validators
{
    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime bookingDate)
            {
                DateTime currentDate = DateTime.Now;

                if(bookingDate < currentDate)
                {
                    return new ValidationResult("Booking must be in future");
                }
            }

            return ValidationResult.Success;
        }
    }
}
