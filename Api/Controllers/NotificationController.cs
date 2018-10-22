using Business.Implementation;
using Contract.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Api.Controllers
{
    public class NotifySubscription
    {
        public string idApp { get; set; }
        public string idPlan { get; set; }
        public string addDays { get; set; }
        public string idUserExternal { get; set; }
    }

    public class NotifySubscriptionPromo
    {
        public string idApp { get; set; }
        public string idPlan { get; set; }
        public string addDays { get; set; }
        public string idUserExternal { get; set; }
        public string tagPromo { get; set; }
    }

    public class NotifyPremio
    {
        public string idApp { get; set; }
        public string idUserExternal { get; set; }
        public string premio { get; set; }
    }

    public class NotifyData
    {
        public string idApp { get; set; }
        public string idPlan { get; set; }
        public string idUserExternal { get; set; }

        public string result { get; set; }
        public string codExternal { get; set; }
        public string message { get; set; }
    }

    public class BillingNotifyParameters
    {
        public string id { get; set; }
        public string idStage { get; set; }
        public string idCommerce { get; set; }
        public string idPaymentType { get; set; }
        public string trx { get; set; }
        public string amount { get; set; }
        public string idUserExternal { get; set; }
        public string idApp { get; set; }
        public string idPlan { get; set; }
        public string idCountry { get; set; }
        public string urlOk { get; set; }
        public string urlError { get; set; }
        public string urlNotify { get; set; }
        public string creationDate { get; set; }
        public string modificationDate { get; set; }
    }

    [RoutePrefix("api/notification")]
    public class NotificationController : ApiController
    {
        private ISubscriptionsService subscriptionsService;

        public NotificationController(ISubscriptionsService _subscriptionsService)
        {
            this.subscriptionsService = _subscriptionsService;
        }
        
        /// <summary>
        /// Notificacion de seteo de suscripcion
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("billing")]
        public bool BillingNotify([FromBody]NotifyData data)
        {
            // Contiene los parámetros necesarios para la suscripción
            // idApp, idPlan, idUserExternal
            // result, codExternal, message
            if((data.result == null || data.result == "")
                && (data.codExternal == null || data.codExternal == "")
                && (data.message == null || data.message == "")) {
                return subscriptionsService.SetSubscription(data.idUserExternal, int.Parse(data.idPlan), data.idApp);
            } else {
                // Flujo nuevo
                string codExternal = data.codExternal;
                string[] arr = codExternal.Split('-');
                string idUserExternal = arr[2];
                string idPlan = arr[1];
                string idApp = arr[3];

                return subscriptionsService.SetSubscription(idUserExternal, int.Parse(idPlan), idApp);
            }
            
        }

        /// <summary>
        /// Notificación de seteo de suscripción, solo si la respuesta es efectiva
        /// Debe ser normalizado todo a esta respuesta
        /// </summary>
        /// <param name="data">Parámetros de respuesta del motor de pagos hacia freemium</param>
        /// <returns></returns>
        [HttpPost]
        [Route("billing2")]
        public bool BillingNotify2([FromBody]NotifyData data) {

            // Acción de suscripción en función de resultado de motor
            if(data.result == "1") {

                // OK, se intenta suscribir a perfil seleccionado
                // Contiene los parámetros necesarios para la suscripción
                string codExternal = data.codExternal;
                string[] arr = codExternal.Split('-');
                string idUserExternal = arr[2];
                string idPlan = arr[1];
                string idApp = arr[3];

                return subscriptionsService.SetSubscription(idUserExternal, int.Parse(idPlan), idApp);

            } else {

                return false;

            }

            
        }


        [HttpPost]
        [Route("cancel")]
        public bool CanelNotify([FromBody]NotifyData data)
        {
            string codExternal = data.codExternal;
            string[] arr = codExternal.Split('-');
            string idUserExternal = arr[2];
            string idPlan = arr[1];
            string idApp = arr[3];

            //return subscriptionsService.SetSubscription(data.idUserExternal, int.Parse(data.idPlan), data.idApp);
            (new Repository.Implementation.EventLogRepository()).SetLog("Usuario cancela suscripcion APP[" + idApp + "] USER[" + idUserExternal + "] PLAN[" + idPlan + "]","Cancelacion de suscripcion");
            return true;
        }


        [HttpPost]
        [Route("inittester")]
        public string InitTransactionTester(
         string IDApp,
         string IDPlan,
         string IDCountry,
         string UrlOk,
         string UrlError,
         string UrlNotify,
         string CommerceID
            )
        {
            return "hola, probando";
        }


        //setea la suscripcion por notify
        [HttpPost]
        [Route("setsubscription")]
        public bool SetSubscription([FromBody]NotifySubscription data)
        {
            (new Repository.Implementation.EventLogRepository()).SetLog("idApp["+data.idApp+"] idPlan["+data.idPlan+"] idUserExternal["+data.idUserExternal+"] addDays["+data.addDays+"] ","Parametros entrada");

            try
            {
                return subscriptionsService.SetSubscription(data.idUserExternal, int.Parse(data.idPlan), data.idApp, int.Parse(data.addDays));
            }
            catch (Exception ex)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog(ex.Message,"Exception");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message));
            }
        }

        //setea la suscripcion por notify
        [HttpPost]
        [Route("setpremio")]
        public bool SetPremio([FromBody]NotifyPremio data)
        {
            (new Repository.Implementation.EventLogRepository()).SetLog("PREMIO: PREMIO[" + data.premio + "] idUserExternal[" + data.idUserExternal + "] idApp[" + data.idApp + "]", "Ingreso de premio");
            return true;
        }

        //setea la suscripcion por notify + promo
        [HttpPost]
        [Route("setsubscriptionpromo")]
        public bool SetSubscription([FromBody]NotifySubscriptionPromo data)
        {
            (new Repository.Implementation.EventLogRepository()).SetLog("SetSubscriptionPromo: idApp[" + data.idApp + "] idPlan[" + data.idPlan + "] idUserExternal[" + data.idUserExternal + "] addDays[" + data.addDays + "] promo[" + data.tagPromo+ "]", "Parametros entrada");

            try
            {
                return subscriptionsService.SetSubscriptionWithPromo(data.idUserExternal, int.Parse(data.idPlan), data.idApp,data.tagPromo);
            }
            catch (Exception ex)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog(ex.Message, "Exception");
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message));
            }
        }
    }
}