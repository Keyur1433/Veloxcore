using System.Runtime.Serialization;

namespace APZMS.Domain.Exceptions
{
    public class ActivityNotFoundException : DomainException
    {
        public ActivityNotFoundException()
        : base("The requested activity was not found.") { }

        public ActivityNotFoundException(string message)
            : base(message) { }

        public ActivityNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        protected ActivityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
