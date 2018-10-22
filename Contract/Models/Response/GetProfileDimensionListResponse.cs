using Contract.Models.Dto;
using System.Collections.Generic;

namespace Contract.Models.Response
{
    public class GetProfileDimensionListResponse
    {
        public int totalItems { get; set; }
        public List<ProfileDimension> items { get; set; }
    }
}
