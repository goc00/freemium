using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class Users
    {
        public string ExternalCode { get; set; }

        //public int? IdProduct { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        //public string Password { get; set; }

        public bool? Active { get; set; }
    }
}
