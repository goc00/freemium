using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class SubscriptionRequest
    {
        public int? idProduct { get; set; }
        public string idUserExternal { get; set; }
        public int? idProfile { get; set; }
    }
}
