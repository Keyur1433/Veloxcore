using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace APZMS.Domain.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        protected DomainException() { }

        protected DomainException(string message) : base(message) { }

        protected DomainException(string message, Exception innerException) : base(message, innerException) { }

        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
