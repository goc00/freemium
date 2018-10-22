using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public enum RestCode
    {
        Ok = 1,
        Error = 0
    }

    /// <summary>
    /// 
    /// </summary>
    public class RestResponse
    {
        /// <summary>
        /// Response Code
        /// </summary>
        public RestCode Code { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }
    }
}
