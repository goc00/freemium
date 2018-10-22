using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class UsersDimensions
    {
        public int IdDimension { get; set; }
        public string Dimension { get; set; }
        
        public decimal CurrentValue { get; set; }

        public string TagName { get; set; }
    }
}
