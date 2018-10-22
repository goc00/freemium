namespace Contract.Models.Request
{
    public class SetDimensionRequest
    {
        public int      idProduct           { get; set; } = -1;
        public string   description         { get; set; }
        public int      idDimensionType     { get; set; } = -1;
        public int      idDimensionCategory { get; set; } = -1;
        public decimal  value               { get; set; } = -1;
        public int      switchValue         { get; set; } = -1;
        public int      active              { get; set; } = 1;
        public string   tagName             { get; set; }
    }
}
