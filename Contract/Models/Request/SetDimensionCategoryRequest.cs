using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class SetDimensionCategoryRequest
    {
        public string description { get; set; }
        public int idProduct { get; set; }
        public string tagName { get; set; }
        public int active { get; set; } = -1;
    }
}
