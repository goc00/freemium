using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetDimensionListResponse
    {
        public int totalItems { get; set; }
        public List<SimpleDimension> items { get; set; }
    }
}
