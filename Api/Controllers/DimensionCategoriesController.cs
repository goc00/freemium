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
    [RoutePrefix("api/v1/dimensioncategories")]
    public class DimensionCategoriesController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Business layer
        DimensionCategoriesService core = new DimensionCategoriesService();

        /// <summary>
        /// Get All Dimension Types
        /// </summary>
        /// <returns></returns>
        [Route("product/{idProduct:int}"), HttpGet]
        public IHttpActionResult GetDimensionCategories([FromUri]GetDimensionCategoriesByIdProductRequest requestObj)
        {
            try
            {
                // Verify required parameters
                if (requestObj.idProduct == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetDimensionCategoriesAction(requestObj.idProduct);

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
        /// Get Dimension Category by Product and (Category)
        /// </summary>
        /// <param name="requestObj"></param>
        /// <returns></returns>
        [Route("product/{idProduct:int}/criteria/{criteria}"), HttpGet]
        public IHttpActionResult GetDimensionCategoriesByCriteria([FromUri]GetDimensionTypesByIdProductAndIdDTRequest requestObj)
        {
            try
            {
                // Verify required parameters
                if (requestObj.idProduct == 0 || String.IsNullOrEmpty(requestObj.criteria))
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetDimensionCategoryByCriteriaAction(requestObj.idProduct, requestObj.criteria);

                if (action.code == (int)CodeStatusEnum.OK) // OK
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

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="requestObj">SetDimensionTypeRequest object</param>
        /// <returns></returns>
        [Route("product/{idProduct:int}"), HttpPost]
        public IHttpActionResult SetDimensionCategoryAction([FromUri]GetDimensionCategoriesByIdProductRequest uriObj, [FromBody]SetDimensionCategoryRequest requestObj)
        {
            try
            {
                // Verify at least object arrives with data
                if (requestObj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify required parameters
                if (String.IsNullOrEmpty(requestObj.description) || uriObj.idProduct == 0 || String.IsNullOrEmpty(requestObj.tagName))
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                ActionResponse action = core.SetDimensionCategoryAction(
                    uriObj.idProduct,
                    requestObj
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

        [Route("{idDimensionCategory:int}"), HttpPut]
        public IHttpActionResult UpdateDimensionCategoryAction([FromUri]UpdateDimensionCategoryRequest uriObj, [FromBody]UpdateDimensionCategoryRequest requestObj)
        {
            try
            {
                // Verify at least object arrives with data
                if (uriObj == null || requestObj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Business layer
                ActionResponse action = core.UpdateDimensionCategoryAction(
                    uriObj.idDimensionCategory,
                    requestObj
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
