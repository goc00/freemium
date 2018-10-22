namespace Contract.Models.Response.Common
{

    public class JsonResponseError {
        public string apiVersion { get; set; } = "1.0";
        public string context { get; set; } = "freemium";
        public JsonResponseErrorParams error { get; set; } = null;
    }

}