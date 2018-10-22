using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Response
{
    public class UpdateValueDimensionResponse
    {
        public int idDimension { get; set; }
        public string nameDimension { get; set; }
        public string tagName { get; set; }
        public int idDimensionType { get; set; }
        public string nameDimensionType { get; set; }
        public Object oldValue { get; set; }
        public Object currentValue { get; set; }
    }
}
