using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class GetDimensionTypesByIdProductAndIdDTRequest
    {
        public int idProduct { get; set; } = 0;
        public string criteria { get; set; }
    }
}
