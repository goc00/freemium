using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Request
{
    public class SetProfileUserRequest
    {
        public int idProduct { get; set; } = 0;
        public int idClient { get; set; } = 0;
        public int idProfile { get; set; } = 0;
        public object idGuide { get; set; } = 0; // optional, can receive any value
    }
}
