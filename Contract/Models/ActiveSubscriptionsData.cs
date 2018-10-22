using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class ActiveSubscriptionsData
    {
        public Nullable<DateTime> SubscriptionDateCreated { get; set; }
        public Nullable<DateTime> SubscriptionDateLastRenewal { get; set; }
        public string UserExternalCode { get; set; }

        public Nullable<int> ProfileId { get; set; }
        public string ProfileName { get; set; }
        public Nullable<bool> ProfilePaid { get; set; }
    }
}
