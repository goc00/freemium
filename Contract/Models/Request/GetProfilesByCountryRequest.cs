namespace Contract.Models.Request
{
    public class GetProfilesByCountryRequest
    {
        public int idProduct { get; set; } = 0;
        public string alpha2 { get; set; } = "";
    }
}
