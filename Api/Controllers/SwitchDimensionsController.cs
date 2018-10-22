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
    [RoutePrefix("api/switchdimensions")]
    public class SwitchDimensionsController : ApiController
    {
        private IDimensionsService dimensionsService;

        public SwitchDimensionsController(IDimensionsService _DimensionsService)
        {
            this.dimensionsService = _DimensionsService;
        }

        [HttpGet]
        [Route("{ProductToken}/{UserCode}/{DimensionTag}")]
        public bool Get(string ProductToken, string UserCode, string DimensionTag)
        {
            try
            {
                return this.dimensionsService.GetSwitchDimension(ProductToken, UserCode, DimensionTag);
            }
            catch (Contract.Exceptions.ProductsNotFoundException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message));
            }
            catch (Contract.Exceptions.UserNotFoundException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
            catch (Contract.Exceptions.ProductDimensionNotFoundException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
            catch (Contract.Exceptions.ProfileDimensionNoDefaultValueException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }
            catch (Contract.Exceptions.ProfileDimensionAnonNoConfiguredException ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message));
            }

            
        }
    }
}