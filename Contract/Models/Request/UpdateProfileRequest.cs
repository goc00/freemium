namespace Contract.Models.Request
{
    public class UpdateProfileRequest
    {
        public int      idProfile   { get; set; } = 0;
        public int      idProduct   { get; set; } = 0;
        public string   name        { get; set; }
        public string   description { get; set; }
        public decimal  priceUSD    { get; set; } = 0;
        public string   tagName     { get; set; }
        public int      anonDefault { get; set; } = -1;
        public int      userDefault { get; set; } = -1;
        public int      paid        { get; set; } = -1;
        public int      featured    { get; set; } = -1;
        public int      active      { get; set; } = -1;
    }
}
