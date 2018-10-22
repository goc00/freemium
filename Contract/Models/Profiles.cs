using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class Profiles
    {
        public int ProfileId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

        public string TagName { get; set; }

        public bool? Paid { get; set; }

        //public bool? AnonDefault { get; set; }

        //public bool? UserDefault { get; set; }
    }
}
