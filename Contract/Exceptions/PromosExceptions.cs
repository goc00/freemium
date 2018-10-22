using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Exceptions
{
    public class PromoNotFoundException : Exception
    {
        public PromoNotFoundException(string message)
            : base(message)
        {
        }

        public PromoNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
