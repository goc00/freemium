using Contract.Models.Response.Common;
using System.Web.Http;

namespace Api.Controllers
{
    public class GeneralController : ApiController
    {

        /// <summary>
        /// OK Response for service
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        protected IHttpActionResult ResponseOk(object res)
        {
            JsonResponseOk result = new JsonResponseOk();
            result.data = res;

            return Ok(result);
        }

        /// <summary>
        /// Error Response
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IHttpActionResult ResponseError(int code, string message)
        {
            JsonResponseErrorParams resultParams = new JsonResponseErrorParams();
            resultParams.code = code;
            resultParams.message = message;

            JsonResponseError result = new JsonResponseError();
            result.error = resultParams;

            return Ok(result);
        }

    }
}
