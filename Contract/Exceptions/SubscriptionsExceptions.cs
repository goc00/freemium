using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class SubscriptionNotFoundException : Exception
    {
        public SubscriptionNotFoundException(string message) : base(message)
        {
        }

        public SubscriptionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
