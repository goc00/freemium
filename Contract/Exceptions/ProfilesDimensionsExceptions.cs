using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class ProfileDimensionNotConfiguredException : Exception
    {
        public ProfileDimensionNotConfiguredException(string message) : base(message)
        {
        }

        public ProfileDimensionNotConfiguredException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

    public class ProfileDimensionNoDefaultValueException : Exception
    {
        public ProfileDimensionNoDefaultValueException(string message) : base(message)
        {
        }

        public ProfileDimensionNoDefaultValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

    public class ProfileDimensionAnonNoConfiguredException : Exception
    {
        public ProfileDimensionAnonNoConfiguredException(string message) : base(message)
        {
        }

        public ProfileDimensionAnonNoConfiguredException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
