using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contract.Exceptions
{
	public class NotEnoughAttributesException : Exception {

        public NotEnoughAttributesException():base() { }
        public NotEnoughAttributesException(string message): base(message) { }

    }
}