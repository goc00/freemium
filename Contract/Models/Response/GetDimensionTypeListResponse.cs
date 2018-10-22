using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetDimensionTypeListResponse
    {
        public int totalItems { get; set; }
        public List<DimensionType> items { get; set; }
    }
}
