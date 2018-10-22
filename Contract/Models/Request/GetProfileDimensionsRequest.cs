using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class GetProfileDimensionsRequest
    {
        public int idProduct { get; set; } = -1;
        public int? idProfile { get; set; } = -1;
        public int? idDimension { get; set; } = -1;
        public decimal? value { get; set; } = -1;
        public int? switchValue { get; set; } = -1;
        public int? active { get; set; } = -1;
    }
}
