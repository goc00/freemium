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
    [RoutePrefix("api/dimension")]
    public class DimensionController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Obtain all dimension data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getdimension/{idProduct:int}/{idClient:int}/{idProfile:int}/{idDimension:int}")]
        public IHttpActionResult GetDimension([FromUri]GetDimensionRequest obj)
        {
            try
            {
                // Business layer
                DimensionsService core = new DimensionsService();

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idProduct == 0 || obj.idClient == 0 || obj.idProfile == 0 || obj.idDimension == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetDimensionAction(obj.idProduct,
                                                                obj.idClient,
                                                                obj.idProfile,
                                                                obj.idDimension);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
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
        /// Update or set value for dimension
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateValueDimension([FromBody]UpdateValueDimensionRequest obj)
        {
            try
            {
                // Business layer
                DimensionsService core = new DimensionsService();

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idProduct == 0 || obj.idClient == 0 || obj.idProfile == 0 || obj.idDimension == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.UpdateValueDimensionAction(obj.idProduct,
                                                                obj.idClient,
                                                                obj.idProfile,
                                                                obj.idDimension,
                                                                obj.value);

                if(action.code == (int)CodeStatusEnum.OK) { return ResponseOk(action.data); }  // OK
                else { return ResponseError(action.code, action.message); } // NOK
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


        [Route("product/{idProduct:int}"), HttpGet]
        public IHttpActionResult GetDimensions([FromUri]GetDimensionRequest request)
        {
            try
            {
                // Verify required parameters
                if (request.idProduct == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                DimensionsService core = new DimensionsService();

                // Actions into business layer
                ActionResponse action = core.GetDimensionsByProductAction(request.idProduct);

                if (action.code == (int)CodeStatusEnum.OK) // Success
                {
                    return ResponseOk(action.data);
                }
                else // Error
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

        [Route("product/{idProduct:int}"), HttpPost]
        public IHttpActionResult SetDimension([FromUri]SetDimensionRequest uri, [FromBody]SetDimensionRequest body)
        {
            try
            {
                if (uri == null || body == null) // Check all data
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                if (uri.idProduct == -1) // Check Product
                {
                    throw new NotEnoughAttributesException("Producto no recibido");
                }

                // Check all required params
                if (String.IsNullOrEmpty(body.description) || body.idDimensionType == -1
                    || body.idDimensionCategory == -1 || body.active == -1 
                    || (body.value == -1 && body.switchValue == -1) || String.IsNullOrEmpty(body.tagName) )
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                DimensionsService core = new DimensionsService();
                
                ActionResponse action = core.SetDimensionAction(
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

        [Route("{idDimension:int}"), HttpPut]
        public IHttpActionResult UpdateDimension([FromUri]UpdateDimensionRequest uri, [FromBody]UpdateDimensionRequest body)
        {
            try
            {
                // Verify at least object arrives with data
                if (uri == null || body == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                if (body.idProduct == -1)
                {
                    throw new NotEnoughAttributesException("Producto no recibido");
                }

                // Business layer
                DimensionsService core = new DimensionsService();

                // Business layer
                ActionResponse action = core.UpdateDimensionAction(
                    uri.idDimension,
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
    }
}
