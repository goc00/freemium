using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class Subscriptions
    {

        //datos de la suscripcion
        public int IdSubscription { get; set; }

        public string ExternalCode { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? RenewalDay { get; set; }

        public DateTime? LastRenewal { get; set; }

        public bool? IsCurrent { get; set; }

        //relaciones

        public Profiles Profiles { get; set; }

        public Users Users { get; set; }

        //datos de promo

        public int? PromoFreeDays { get; set; }

        public DateTime? PromoStarted { get; set; }

        public DateTime? PromoEnd { get; set; }

        public bool? PromoActive { get; set; }

        public int? IdPromo { get; set; }

        public List<UsersDimensions> Dimensions { get; set; }
    }
}
