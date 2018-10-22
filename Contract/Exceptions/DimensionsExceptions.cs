using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class ProductDimensionNotFoundException : Exception
    {
        public ProductDimensionNotFoundException(string message) : base(message)
        {
        }

        public ProductDimensionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
