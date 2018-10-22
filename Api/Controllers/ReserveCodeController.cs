using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/reservecode")]
    public class ReserveCodeController : ApiController
    {

        /// <summary>
        /// Notificacion de seteo de suscripcion
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generatecode")]
        public bool GenerateCode([FromBody]NotifyData data) {
            // Contiene los parámetros necesarios para la suscripción
            // idApp, idPlan, idUserExternal
            // result, codExternal, message
            /*if((data.result == null || data.result == "")
                && (data.codExternal == null || data.codExternal == "")
                && (data.message == null || data.message == "")) {
                return subscriptionsService.SetSubscription(data.idUserExternal, int.Parse(data.idPlan), data.idApp);
            }*/
            return false;
        }

    }
}
