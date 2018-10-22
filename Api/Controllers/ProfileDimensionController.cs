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
    [RoutePrefix("api/v1/profiledimensions")]
    public class ProfileDimensionsController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Business layer
        ProfileDimensionsService core = new ProfileDimensionsService();

        /// <summary>
        /// Get All Dimension Types
        /// </summary>
        /// <returns></returns>
        [Route("{idProduct:int}"), HttpGet]
        public IHttpActionResult GetProfileDimensions([FromUri]GetProfileDimensionsRequest uri)
        {
            try
            {
                // Verify required parameters
                if (uri.idProduct == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetProfileDimensionsByProductAction(uri.idProduct);

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

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="requestObj">SetDimensionTypeRequest object</param>
        /// <returns></returns>
        [Route("{idProduct:int}"), HttpPost]
        public IHttpActionResult SetProfileDimension([FromUri]GetProfileDimensionsRequest uri, [FromBody]GetProfileDimensionsRequest body)
        {
            try
            {
                // Verify at least object arrives with data
                if (uri == null || body == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify required parameters
                if (uri.idProduct == -1 || body.idProfile == -1 || body.idDimension == -1 || body.value == -1 || body.switchValue == -1 || body.active == -1)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                ActionResponse action = core.SetProfileDimensionAction(
                    uri.idProduct, 
                    body
                );

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

        [Route("{idProfileDimension:int}"), HttpPut]
        public IHttpActionResult UpdateDimensionCategoryAction(int? idProfileDimension, [FromBody]GetProfileDimensionsRequest body)
        {
            try
            {
                // Verify at least object arrives with data
                if (idProfileDimension == null || body == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Business layer
                ActionResponse action = core.UpdateProfileDimensionAction(
                    idProfileDimension.Value,
                    body
                );

                if (action.code == (int)CodeStatusEnum.OK)
                {
                    UpdateResponse ur = new UpdateResponse();
                    ur.updated = DateTime.Now;
                    ur.totalItemsUpdated = 1;
                    return ResponseOk(ur);
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
