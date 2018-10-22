using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class GetSubscriptionsByRenewalParams
    {
        public string currentDate { get; set; }
        public int top { get; set; }
    }
}
