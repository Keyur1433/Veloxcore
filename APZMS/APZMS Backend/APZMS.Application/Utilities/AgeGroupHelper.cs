namespace APZMS.Application.Common
{
    public class AgeGroupHelper
    {
        public static string GetAgeGroup(DateTime dob)
        {
            var today = DateTime.Now;   
            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age)) age--;

            return age switch
            {
                >= 2 and <= 4 => "toddler",
                >= 5 and <= 12 => "kid",
                >= 13 and <= 17 => "teen",
                _ => "adult"
            };
        }
    }
}
