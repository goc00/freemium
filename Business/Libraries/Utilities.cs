using Contract.Models.Response.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Libraries
{
    public class Utilities
    {
        /// <summary>
        /// General response for any action into module
        /// </summary>
        /// <param name="code">0 = OK, anything distinct to 0, will be considered an error</param>
        /// <param name="message">Possible error message</param>
        /// <param name="data">Any object which one will be retorned</param>
        /// <returns></returns>
        public ActionResponse Response(int code, string message, object data)
        {
            ActionResponse response = new ActionResponse();
            response.code = code;
            response.message = message;
            response.data = data;

            return response;
        }
    }
}
