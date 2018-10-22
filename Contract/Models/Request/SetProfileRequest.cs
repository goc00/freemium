using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class SetProfileRequest
    {
        public int IdProduct { get; set; } = 0;
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceUSD { get; set; } = 0;
        public string TagName { get; set; }
        public bool AnonDefault { get; set; }
        public bool UserDefault { get; set; }
        public bool Paid { get; set; }
        public bool Featured { get; set; }
    }
}
