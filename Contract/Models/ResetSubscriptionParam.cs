using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class ResetSubscriptionParam
    {
        public string ProductToken { get; set; }
        public string CategoryNameTag { get; set; }
        public string UserExternalCode { get; set; }
    }
}
