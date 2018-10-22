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
    [RoutePrefix("api/profile")]
    public class ProfileController : GeneralController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Business layer
        ProfileService core = new ProfileService();

        /// <summary>
        /// Get profiles by product
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getprofilesbyproduct/{idProduct:int}")]
        public IHttpActionResult GetProfilesByProduct([FromUri]GetProfilesByProductRequest obj)
        {
            try
            {
      
                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idProduct == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetProfilesByProductAction(obj.idProduct);

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
        /// Get profiles by country
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getprofilesbycountry/{idProduct:int}/{alpha2}")]
        public IHttpActionResult GetProfilesByCountry([FromUri]GetProfilesByCountryRequest obj)
        {
            try
            {

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idProduct == 0 || String.IsNullOrEmpty(obj.alpha2.Trim()))
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Actions into business layer
                ActionResponse action = core.GetProfilesByCountryAction(obj.idProduct, obj.alpha2.ToUpper());

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
        /// Set profile (and instance dimensions if needed) to user
        /// </summary>
        /// <param name="obj">SetProfileUserRequest object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SetProfileUser([FromBody]SetProfileUserRequest obj)
        {
            try
            {

                // Verify at least object arrives with data
                if(obj == null) throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");

                // Verify for parameters needed
                if(obj.idProduct == 0 || obj.idClient == 0 || obj.idProfile == 0)
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Check id guide, it can be any value, then if it's something different to int or decimal, will be
                // equals to zero (0)
                int idGuide = 0;
                if(obj.idGuide != null)
                {

                    int Num;
                    bool isNum = int.TryParse(obj.idGuide.ToString(), out Num);
       
                    if(!isNum)
                    {
                        idGuide = 0;
                    }
                    else
                    {
                        idGuide = Convert.ToInt32(obj.idGuide);
                    }
                }
                

                // Actions into business layer
                ActionResponse action = core.SetProfileUserAction(obj.idProduct,
                                                                    obj.idClient,   // ID user master (NO id user freemium)
                                                                    obj.idProfile,
                                                                    idGuide);   // optional

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
        /// Store new record
        /// </summary>
        /// <param name="uriObj"></param>
        /// <param name="bodyObj"></param>
        /// <returns></returns>
        [Route("product/{idProduct:int}"), HttpPost]
        public IHttpActionResult SetProfileAction([FromUri]GetProfilesByProductRequest uriObj, [FromBody]SetProfileRequest bodyObj)
        {
            try
            {
                // Verify at least object arrives with data
                if (bodyObj == null)
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                // Verify required parameters
                if (String.IsNullOrEmpty(bodyObj.Name) || String.IsNullOrEmpty(bodyObj.Description) || String.IsNullOrEmpty(bodyObj.TagName))
                {
                    throw new NotEnoughAttributesException("No se han recibido todos los parámetros requeridos");
                }

                // Business layer
                ActionResponse action = core.SetProfileAction(
                    uriObj.idProduct,
                    bodyObj
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

        [Route("{idProfile:int}"), HttpPut]
        public IHttpActionResult UpdateProfile([FromUri]UpdateProfileRequest uri, [FromBody]UpdateProfileRequest body)
        {
            try
            {
                if (uri == null || body == null) // Check required params
                {
                    throw new NotEnoughAttributesException("No se ha recibido ningún parámetro");
                }

                if (uri.idProfile == 0) // Check Profile ID
                {
                    throw new NotEnoughAttributesException("No se ha especificado el perfil");
                }

                if (body.idProduct == 0) // Check Product ID
                {
                    throw new NotEnoughAttributesException("No se ha recibido el producto");
                }

                // Business layer
                ActionResponse action = core.UpdateProfileAction(
                    uri.idProfile,
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
