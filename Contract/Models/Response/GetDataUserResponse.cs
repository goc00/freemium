using Contract.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Response
{
    public class GetDataUserResponse
    {
        public int idSubscription { get; set; }
        public DateTime dateCreated { get; set; }
        public Profile profile { get; set; }
        public List<Dimension> dimensions { get; set; }
    }
}
