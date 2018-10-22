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
    [RoutePrefix("api/numericdimensions")]
    public class NumericDimensionsController : ApiController
    {
        private IDimensionsService DimensionsService;

        public NumericDimensionsController(IDimensionsService _DimensionsService)
        {
            this.DimensionsService = _DimensionsService;
        }

        [HttpGet]
        [Route("{ProductToken}/{UserCode}/{DimensionTag}")]
        public decimal Get( string ProductToken, string UserCode, string DimensionTag)
        {
            /* VALIDACIONES */
            if (UserCode == "" || UserCode == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"UserCode\" no puede estar vacío"));

            if (ProductToken == "" || ProductToken == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"ProductToken\" no puede estar vacío"));

            if (DimensionTag == "" || DimensionTag == String.Empty)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, "Parámetro \"DimensionTag\" no puede estar vacío"));

            return this.DimensionsService.GetNumericDimension(ProductToken, UserCode, DimensionTag);
        }
    }
}
