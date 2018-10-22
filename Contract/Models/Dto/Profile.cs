using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Dto
{
    public class Profile
    {
        public int idProfile { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool? active { get; set; }
        public string tagName { get; set; }
        public bool? paid { get; set; }
        public string country { get; set; }

        public List<Dimension> dimensions { get; set; }

    }
}
