using Contract.Models;
using Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/numericconsumabledimensions")]
    public class NumericConsumableDimensionsController : ApiController
    {
        private IDimensionsService DimensionsService;

        public NumericConsumableDimensionsController(IDimensionsService _DimensionsService)
        {
            this.DimensionsService = _DimensionsService;
        }

        [HttpGet]
        [Route("{ProductToken}/{UserCode}/{DimensionTag}")]
        public decimal Get(string ProductToken, string UserCode, string DimensionTag)
        {
            /* VALIDACIONES */
            if (UserCode == "" || UserCode == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"UserCode\" no puede estar vacío"));

            if (ProductToken == "" || ProductToken == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"ProductToken\" no puede estar vacío"));

            if (DimensionTag == "" || DimensionTag == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"DimensionTag\" no puede estar vacío"));

            return this.DimensionsService.GetNumericConsumableDimension(ProductToken, UserCode, DimensionTag);
        }

        [HttpPut]
        [Route("{ProductToken}/{UserCode}/{DimensionTag}/{amount}")]
        public decimal Consume(string ProductToken, string UserCode, string DimensionTag, decimal Amount)
        {
            /* VALIDACIONES */
            if (UserCode == "" || UserCode == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"UserCode\" no puede estar vacío"));

            if (ProductToken == "" || ProductToken == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"ProductToken\" no puede estar vacío"));

            if (DimensionTag == "" || DimensionTag == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"DimensionTag\" no puede estar vacío"));

            return this.DimensionsService.ConsumeNumericConsumableDimension(ProductToken, UserCode, DimensionTag, Amount);
        }
    }
}
