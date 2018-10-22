using System;

namespace Contract.Models.Response.Common
{

    public class UpdateResponse
    {
        public DateTime updated { get; set; }
        public int totalItemsUpdated { get; set; } = 0;
    }

}