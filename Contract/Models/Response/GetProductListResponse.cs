using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetProductListResponse
    {
        public int totalItems { get; set; }
        public List<Product> items { get; set; }
    }
}
