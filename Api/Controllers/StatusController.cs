using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Contract.Interfaces;
using Microsoft.Practices.Unity;
using System.Web.Script.Serialization;
using System.Web.Mvc;


namespace Api.Controllers
{
    public class StatusController : ApiController
    {

        private IStatusService StatusService;

        public StatusController(IStatusService _StatusService)
        {
            this.StatusService = _StatusService;
        }

        // GET: api/Status
        public List<string> Get()
        {
            List<string> listado = this.StatusService.Get().ToList();
            /*
            string resultado = new JavaScriptSerializer().Serialize(listado);

            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "No estás autorizado a utilizar esta API"));
    
            return new JavaScriptSerializer().Serialize(listado);*/

            return listado;
        }

    }
}
