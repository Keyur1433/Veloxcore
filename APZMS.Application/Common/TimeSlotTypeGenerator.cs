namespace APZMS.Application.Common
{
    public class TimeSlotTypeGenerator
    {
        public static string GetTimeSlotType(string bookingHours)
        {
            // Try parsing the time from the bookingHours string
            if (DateTime.TryParse(bookingHours, out DateTime bookingTime))
            {
                int hour = bookingTime.Hour;

                // Check the time slot based on the hour
                if (hour >= 10 && hour < 14)
                {
                    return "Off-Peak";
                }
                else if ((hour >= 14 && hour < 18) || (hour >= 9 && hour < 10))
                {
                    return "Standard";
                }
                else if (hour >= 18 && hour < 22)
                {
                    return "Peak";
                }
                else
                {
                    return "Out of available time slots";
                }
            }
            else
            {
                // If the time format is invalid, return an error message
                return "Invalid time format.";
            }
        }
    }
}
