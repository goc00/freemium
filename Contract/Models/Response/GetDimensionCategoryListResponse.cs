using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetDimensionCategoryListResponse
    {
        public int totalItems { get; set; }
        public List<DimensionCategory> items { get; set; }
    }
}
