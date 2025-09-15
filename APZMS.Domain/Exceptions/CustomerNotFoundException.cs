using System.Runtime.Serialization;

namespace APZMS.Domain.Exceptions
{
    public class CustomerNotFoundException : DomainException
    {
        public CustomerNotFoundException()
        : base("The requested customer was not found.") { }

        public CustomerNotFoundException(string message)
            : base(message) { }

        public CustomerNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected CustomerNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
