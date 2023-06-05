using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerplant.Infrastructure.Exceptions
{
    public class FulfillmentException : Exception
    {
        public FulfillmentException(string message) : base(message) { }
    }
}
