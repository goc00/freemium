using System.Collections.Generic;

namespace Contract.Models.Dto
{
    public class Product
    {
        public int idProduct { get; set; }
        public string description { get; set; }
        public string token { get; set; }
        public string tagName { get; set; }

        //public List<DimensionCategory> dimensionCategories { get; set; }
        //public List<Profile> profiles { get; set; }

    }
}
