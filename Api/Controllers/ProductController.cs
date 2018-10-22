using Business.Implementation;
using Contract.Exceptions;
using Contract.Models.Enum;
using Contract.Models.Request;
using Contract.Models.Response.Common;
using NLog;
using System;
using System.Web.Http;

namespace Api.Controllers
{
    [RoutePrefix("api/v1/product")]
    public class ProductController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Business layer
        ProductService core = new ProductService();

        /// <summary>
        /// Get All Dimension Types
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public IHttpActionResult GetProducts()
        {
            try
            {

                // Actions into business layer
                ActionResponse action = core.GetProductsAction();

                if (action.code == (int)CodeStatusEnum.OK) // OK
                {
                    return ResponseOk(action.data);
                }
                else
                {
                    return ResponseError(action.code, action.message);
                }
            }
            catch(NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + ex.Message);
            }
        }
    }
}
