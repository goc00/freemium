using System.Collections.Generic;

namespace Contract.Models.Dto
{
    public class ProfileDimension
    {
        public int idProfileDimension { get; set; }
        public int? idProfile { get; set; }
        public int? idDimension { get; set; }
        public decimal? value { get; set; }
        public bool? switchValue { get; set; }
        public bool? active { get; set; }
        public int? idProduct { get; set; }
    }
}
