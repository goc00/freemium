using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Dto
{
    public class Dimension
    {
        public int idDimension { get; set; }
        public string nameDimension { get; set; }
        public string tagDimension { get; set; }
        public int idDimensionCategory { get; set; }
        public string tagDimensionCategory { get; set; }
        public int idDimensionType { get; set; }
        public string nameDimensionType { get; set; }
        public object currentValue { get; set; }
        public object originalValue { get; set; } = null;
    }
}
