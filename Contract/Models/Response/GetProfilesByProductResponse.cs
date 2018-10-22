using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetProfilesByProductResponse
    {
        public int totalItems { get; set; }
        public List<Profile> items { get; set; }
    }
}
