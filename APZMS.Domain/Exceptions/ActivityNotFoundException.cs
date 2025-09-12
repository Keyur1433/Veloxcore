using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APZMS.Domain.Exceptions
{
    public class ActivityNotFoundException : Exception
    {
        public ActivityNotFoundException(string message) : base(message) { } 
    }
}
