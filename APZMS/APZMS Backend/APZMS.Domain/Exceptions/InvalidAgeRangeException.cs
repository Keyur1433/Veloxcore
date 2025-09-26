using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APZMS.Domain.Exceptions
{
    public class InvalidAgeRangeException : Exception
    {
        public InvalidAgeRangeException() : base("Maximum age must be greater than minimum age.") { }
        public InvalidAgeRangeException(string message) : base(message) { }
    }
}
