using System.Runtime.Serialization;

namespace APZMS.Domain.Exceptions
{
    public class BookingNotFoundException : DomainException
    {
        public BookingNotFoundException()
            : base("The requested booking was not found") { }

        public BookingNotFoundException(string message)
            : base(message) { }

        public BookingNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BookingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
