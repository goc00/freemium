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
    [RoutePrefix("api/dimensiontypes")]
    public class DimensionTypesController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Business layer
        DimensionTypesService core = new DimensionTypesService();

        /// <summary>
        /// Get All Dimension Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getdimensiontypes")]
        public IHttpActionResult GetDimensionTypes()
        {
            try
            {
                // Actions into business layer
                ActionResponse action = core.GetDimensionTypesAction();

                if (action.code == (int)CodeStatusEnum.OK) // OK
                {
                    return ResponseOk(action.data);
                }
                else // NOK
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

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="requestObj">SetDimensionTypeRequest object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetDimensionTypesAction([FromBody]SetDimensionTypeRequest requestObj)
        {
            try
            {
                // Verify at least object arrives with data
                if (requestObj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify required parameters
                if (String.IsNullOrEmpty(requestObj.description) || String.IsNullOrEmpty(requestObj.tagName))
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                ActionResponse action = core.SetDimensionTypesAction(requestObj.description, requestObj.tagName);

                if (action.code == (int)CodeStatusEnum.OK)
                {
                    return ResponseOk(action.data);
                }
                else
                {
                    return ResponseError(action.code, action.message);
                }
            }
            catch (NotValidDataException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch (NotEnoughAttributesException e)
            {
                logger.Error(e.Message);
                return ResponseError((int)CodeStatusEnum.BAD_REQUEST, e.Message);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
                return ResponseError((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + ex.Message);
            }
        }
    }
}
