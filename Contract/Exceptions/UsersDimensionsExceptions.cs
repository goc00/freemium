using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class UserDimensionNotFoundException : Exception
    {
        public UserDimensionNotFoundException(string message) : base(message)
        {
        }

        public UserDimensionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
