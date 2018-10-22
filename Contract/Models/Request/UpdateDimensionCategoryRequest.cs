namespace Contract.Models.Request
{
    public class UpdateDimensionCategoryRequest
    {
        public string description { get; set; } = null;
        public int idProduct { get; set; } = 0;
        public string tagName { get; set; } = null;
        public int idDimensionCategory { get; set; } = 0;
        public int active { get; set; } = -1;
    }
}
