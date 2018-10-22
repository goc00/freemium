using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class SubscriptionsResponse
    {
        public int IdSubscription { get; set; }
        public string ExternalCode { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> RenewalDay { get; set; }
        public Nullable<System.DateTime> LastRenewal { get; set; }
        public Nullable<int> IdUser { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> IdProfile { get; set; }
        public Nullable<bool> IsCurrent { get; set; }
        public Nullable<int> PromoFreeDays { get; set; }
        public Nullable<System.DateTime> PromoStarted { get; set; }
        public Nullable<System.DateTime> PromoEnd { get; set; }
        public Nullable<bool> PromoActive { get; set; }
        public Nullable<int> IdPromo { get; set; }
    }
}
