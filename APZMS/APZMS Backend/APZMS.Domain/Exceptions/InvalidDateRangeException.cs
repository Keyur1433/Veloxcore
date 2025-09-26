namespace APZMS.Domain.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException() : base("Both bookingDateFrom and bookingDateTo must be provided for date filtering.") { }
        public InvalidDateRangeException(string message) : base(message) { }
    }
}