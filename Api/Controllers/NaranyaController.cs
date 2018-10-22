using Contract.Models;
using Newtonsoft.Json;
using Repository.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/naranya")]
    public class NaranyaController : ApiController
    {
        private NaranyaNotificationRepository naranyaNotificationRepository { get; set; }

        public NaranyaController()
        {
            this.naranyaNotificationRepository = new NaranyaNotificationRepository();
        }

        public void InternalNotification(NaranyaNotification data)
        {
            try
            {
                string endpoint = string.Empty;
                if (data.ipn_type == @"subscription.new" && data.status == @"active")
                {
                    // En este caso debemos dar de alta y cobrar
                    endpoint = string.Format(@"http://146.82.89.83/Suscripciones/ClientsWS.asmx/Agregar_lista_integrante?ani={0}&id_lista=7417&usuario=plataforma&password=plat2008", data.id_customer);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.Method = "GET";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    endpoint = string.Format(@"http://146.82.89.84/DRM/DRM/DdBill.aspx?resId=41484&distType=UR&opCode=7125&IDNoticia=810837&mobileNumber={0}", data.id_customer);
                    request = (HttpWebRequest)WebRequest.Create(endpoint);
                    response = request.GetResponse() as HttpWebResponse;
                }
                else if (data.ipn_type == @"subscription.cancel" && data.status == @"canceled")
                {
                    // Solo dar de baja
                    endpoint = string.Format(@"http://146.82.89.83/Suscripciones/ClientsWS.asmx/Borrar_lista_integrante?ani={0}&id_lista=7417&usuario=plataforma&password=plat2008", data.id_customer);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.Method = "GET";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                }
                else if (data.ipn_type == @"subscription.new" && data.status == @"active")
                {
                    // Solo cobrar
                    endpoint = string.Format(@"http://146.82.89.84/DRM/DRM/DdBill.aspx?resId=41484&distType=UR&opCode=7125&IDNoticia=810837&mobileNumber={0}", data.id_customer);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.Method = "GET";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                }
            }
            catch (Exception ex)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("Naranya Subscription Notification EXCEPTION", "Mensaje: " + ex.Message);
            }
        }

        [Route("notification")]
        public bool Notification([FromBody]NaranyaNotification data)
        {
            (new Repository.Implementation.EventLogRepository()).SetLog("Naranya Subscription Notification", "Ingresa notificacion de Naranya: Event["+ data.id_event+"] / Trans["+data.id_transaction+"]");

            //almacena notificacion
            this.naranyaNotificationRepository.SaveNewNotification(data);

            string url_ack = "https://ipn.npay.io/verify/" + data.GetUrlParamsTres();

            // POST query to NPay website, containing the same data that we received
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_ack);
            request.Method = "POST";
            request.ContentType = "application/json";

            /*

            var body = JsonConvert.SerializeObject(data);

            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(body);
                streamWriter.Flush();
                streamWriter.Close();
            }

            */

            try
            {

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    Console.WriteLine(String.Format("StatusCode == {0}", response.StatusCode));
                    Console.WriteLine(sr.ReadToEnd());

                    (new Repository.Implementation.EventLogRepository()).SetLog("Naranya Subscription Notification", "Notificacion con ACK exitoso: Event[" + data.id_event + "] / Trans[" + data.id_transaction + "]");

                }

                // Notificación hacia el sistema de cobro interno (Subscripciones)
                InternalNotification(data);

            }
            catch (Exception ex)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("Naranya Subscription Notification EXCEPTION", "Mensaje: " + ex.Message);

            }

            return true;
        }
    }
}
