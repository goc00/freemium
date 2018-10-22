using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class ProductsNotFoundException : Exception
    {
        public ProductsNotFoundException(string message) : base(message)
        {
        }

        public ProductsNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
