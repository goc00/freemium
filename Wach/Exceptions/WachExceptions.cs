using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wach.Exceptions
{
    public class WachException : Exception
    {
        public WachException(string message) : base(message)
        {
        }

        public WachException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class WachSerializationException : Exception
    {
        public WachSerializationException(string message) : base(message)
        {
        }

        public WachSerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class WachConnectionException : Exception
    {
        public WachConnectionException(string message) : base(message)
        {
        }

        public WachConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class WachNoClientConfiguredException : Exception
    {
        public WachNoClientConfiguredException(string message) : base(message)
        {
        }

        public WachNoClientConfiguredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class WachServerErrorException : Exception
    {
        public WachServerErrorException(string message) : base(message)
        {
        }

        public WachServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
