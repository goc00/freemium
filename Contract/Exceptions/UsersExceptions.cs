using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class UserInactiveException : Exception
    {
        public UserInactiveException(string message) : base(message)
        {
        }

        public UserInactiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
