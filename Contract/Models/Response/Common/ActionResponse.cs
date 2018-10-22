namespace Contract.Models.Response.Common
{

    public class ActionResponse {
        public int code { get; set; } = -1;
        public string message { get; set; } = "no actions";
        public object data { get; set; } = null;
    }

}