using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Models.Dto
{
    public class SimpleDimension
    {
        public int      idDimension             { get; set; }
        public string   nameDimension           { get; set; }
        public string   tagDimension            { get; set; }
        public int      idDimensionCategory     { get; set; }
        public string   tagDimensionCategory    { get; set; }
        public int      idDimensionType         { get; set; }
        public string   nameDimensionType       { get; set; }
        public int      idProduct               { get; set; }
        public decimal? value                   { get; set; }
        public decimal? switchValue             { get; set; }
    }
}
