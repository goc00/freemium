namespace Contract.Models.Dto
{
    public class DimensionCategory
    {
        public int idDimensionCategory { get; set; }
        public string description { get; set; }
        public int? idProduct { get; set; }
        public bool? active { get; set; }
        public string tagName { get; set; }
    }
}
