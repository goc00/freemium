namespace Contract.Models.Dto
{
    public class DimensionData
    {
        public int idDimension { get; set; }
        public string description { get; set; }
        public int idDimensionType { get; set; }
        public int idDimensionCategory { get; set; }
        public decimal? value { get; set; }
        public int? switchValue { get; set; }
        public bool active { get; set; }
        public string tagName { get; set; }
    }
}
