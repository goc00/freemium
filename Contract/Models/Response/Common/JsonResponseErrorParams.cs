namespace Contract.Models.Response.Common
{

    public class JsonResponseErrorParams {
        public int code { get; set; } = -1;
        public string message { get; set; } = "no actions";
    }

}