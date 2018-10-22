using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class DimensionTypeIsNotSwitchException : Exception
    {
        public DimensionTypeIsNotSwitchException(string message) : base(message)
        {
        }

        public DimensionTypeIsNotSwitchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DimensionTypeNotConfiguredSwitchException : Exception
    {
        public DimensionTypeNotConfiguredSwitchException(string message) : base(message)
        {
        }

        public DimensionTypeNotConfiguredSwitchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
