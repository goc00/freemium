namespace Contract.Models.Response.Common
{

    public class JsonResponseOk {
        public string apiVersion { get; set; } = "1.0";
        public string context { get; set; } = "freemium";
        public object data { get; set; } = null;
    }

}