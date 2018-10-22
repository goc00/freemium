using Business.Libraries;
using Contract.Models.Dto;
using Contract.Models.Enum;
using Contract.Models.Response;
using Contract.Models.Response.Common;
using Repository.Implementation;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Business.Implementation
{
    public class ProfileService
    {
        private ProductsRepository productsRepository { get; set; }
        private UsersRepository usersRepository { get; set; }
        private SubscriptionsRepository subscriptionsRepository { get; set; }
        private ProfilesRepository profilesRepository { get; set; }
        private PromosRepository promosRepository { get; set; }
        private DimensionsRepository dimensionsRepository { get; set; }
        private UsersDimensionsRepository usersDimensionsRepository { get; set; }
        private ProfilesDimensionsRepository profilesDimensionsRepository { get; set; }

        private Utilities utilities;

        public ProfileService()
        {
            // Init repositories
            this.productsRepository = new ProductsRepository();
            this.usersRepository = new UsersRepository();
            this.subscriptionsRepository = new SubscriptionsRepository();
            this.profilesRepository = new ProfilesRepository();
            this.promosRepository = new PromosRepository();
            this.dimensionsRepository = new DimensionsRepository();
            this.usersDimensionsRepository = new UsersDimensionsRepository();
            this.profilesDimensionsRepository = new ProfilesDimensionsRepository();

            this.utilities = new Utilities();
        }


        /// <summary>
        /// Get all profiles related to product
        /// </summary>
        /// <param name="idProduct">ID product</param>
        /// <returns></returns>
        public ActionResponse GetProfilesByProductAction(int idProduct)
        {
            
      
            try
            {

                // Find profiles
                var profiles = this.profilesRepository.GetProfiles(idProduct);
                if(profiles.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado perfiles asociados al producto", null);
                }

                List<Profile> lst = this._getProfileDetails(profiles);

                GetProfilesByProductResponse response = new GetProfilesByProductResponse();
                response.totalItems = profiles.Count;
                response.items = lst;

                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }


        /// <summary>
        /// Get all profiles in function of alpha-2
        /// </summary>
        /// <param name="idProduct">ID product</param>
        /// <param name="country">ALPHA-2 from core service (utilities)</param>
        /// <returns></returns>
        public ActionResponse GetProfilesByCountryAction(int idProduct, string alpha2)
        {

            try
            {

                // Find profiles
                var profiles = this.profilesRepository.GetProfilesByCountry(idProduct, alpha2);
                if(profiles.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado perfiles asociados al producto y/o país", null);
                }

                List<Profile> lst = this._getProfileDetails(profiles);

                GetProfilesByCountryResponse response = new GetProfilesByCountryResponse();
                response.totalItems = profiles.Count;
                response.items = lst;

                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="profiles"></param>
        /// <returns></returns>
        private List<Profile> _getProfileDetails(dynamic profiles)
        {
            List<Profile> lst = new List<Profile>();

            foreach(var oProfile in profiles)
            {
                Profile o = new Profile();
                o.idProfile = oProfile.IdProfile;
                o.name = oProfile.Name;
                o.description = oProfile.Description;
                o.active = oProfile.Active;
                o.tagName = oProfile.TagName;
                o.paid = oProfile.Paid;
                o.dimensions = new List<Dimension>();

                // Get all profiles dimensions
                var lstProfileDimensions = this.profilesDimensionsRepository.GetProfileDimensionsByIdProfile(o.idProfile);

                if(lstProfileDimensions.Count > 0)
                {
                    // If we find profiles dimensions, travel to get dimensions 1 per 1
                    foreach(var oProfileDimension in lstProfileDimensions)
                    {
                        var dimension = this.dimensionsRepository.GetDimension(oProfileDimension.IdDimension.Value);
                        Dimension oDimension = new Dimension();
                        oDimension.idDimension = dimension.IdDimension;
                        oDimension.idDimensionCategory = dimension.IdDimensionCategory.Value;
                        oDimension.idDimensionType = dimension.IdDimensionType.Value;
                        oDimension.nameDimension = dimension.Description;
                        oDimension.tagDimension = dimension.TagName;
                        DimensionTypeEnum dimensionTypeEnum = (DimensionTypeEnum)oDimension.idDimensionType;
                        oDimension.nameDimensionType = dimensionTypeEnum.ToString();
                        oDimension.tagDimensionCategory = dimension.DimensionsCategories.TagName;

                        if(dimension.IdDimensionType == (int)DimensionTypeEnum.SWITCH)
                        {
                            oDimension.originalValue = oProfileDimension.SwitchValue;
                        }
                        else
                        {
                            oDimension.originalValue = oProfileDimension.Value;
                        }

                        o.dimensions.Add(oDimension);
                    }

                }


                lst.Add(o);
            }

            return lst;
        }
        
            
        


        /// <summary>
        /// Will set profile to user
        /// </summary>
        /// <param name="idProduct">Product ID</param>
        /// <param name="idClient">User master ID</param>
        /// <param name="idProfile">Profile ID</param>
        /// <param name="idGuide">Guide ID (optional), thought for traceability</param>
        /// <returns></returns>
        public ActionResponse SetProfileUserAction(int idProduct, int idClient, int idProfile, int idGuide)
        {

            try
            {

                // Check if products exists
                var oProduct = this.productsRepository.GetProduct(idProduct);
                if(oProduct == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el producto", null);
                }

                // Check for user
                var oUser = this.usersRepository.GetUserv2(idClient.ToString(), idProduct);
                
                // If user doesn't exist, need to create everything from beggining
                // Will send 0 to process this action
                int idUser = (oUser == null) ? 0 : oUser.IdUser; // ID USER FREEMIUM 

                // Check if profile exists
                var oProfile = profilesRepository.GetProfile(idProfile);
                if(oProfile == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El perfil no existe en el sistema", null);
                }

                // Check if product has profiles associated and idProfile is part of it
                var profiles = oProduct.Profiles;
                if(profiles.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no posee perfiles configurados", null);
                }
                bool isValidProfile = false;
                foreach(var oProf in profiles)
                {
                    if(oProf.IdProfile == idProfile)
                    {
                        isValidProfile = true;
                        break;
                    }
                }

                if(!isValidProfile)
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "El perfil no tiene relación con el producto", null);
                }

                // Set or update all related to profile
                bool action = subscriptionsRepository.SetProfileUser(idProfile, idUser, idClient, idProduct);
                
                if(action)
                {
                    // OK
                    SetProfileUserResponse response = new SetProfileUserResponse
                    {
                        profile = new Profile
                        {
                            idProfile = oProfile.IdProfile,
                            name = oProfile.Name,
                            description = oProfile.Description,
                            tagName = oProfile.TagName,
                            active = oProfile.Active,
                            paid = oProfile.Paid
                        }
                    };


                    // After set user, need to call to user's service to update
                    // Will get user data for getting dimension related to id_lista which will be setted

                    SubscriptionsService subscriptionService = new SubscriptionsService();
                    ActionResponse subscriptionServiceAction = subscriptionService.GetDataUserAction(idProduct, idClient);
                    if(subscriptionServiceAction.code == (int)CodeStatusEnum.OK)
                    {
                        GetDataUserResponse resp = (GetDataUserResponse)subscriptionServiceAction.data;
                        int l = resp.dimensions.Count;
                        if(l > 0)
                        {
                            // Check dimensions to get id list (if it applies)
                            foreach(Dimension obj in resp.dimensions)
                            {
                                if(obj.tagDimension == System.Configuration.ConfigurationManager.AppSettings["TAG_DIMENSION"])
                                {
                                    DimensionsService dimensionService = new DimensionsService();
                                    ActionResponse dimensionServiceAction = dimensionService.GetDimensionAction(idProduct, idClient, idProfile, obj.idDimension);

                                    if(dimensionServiceAction.code == (int)CodeStatusEnum.OK)
                                    {
                                        GetDimensionResponse respB = (GetDimensionResponse)dimensionServiceAction.data;
                                        int idList = Convert.ToInt32(respB.currentValue);

                                        // Call for external service
                                        var test = CallExternal(idClient, idList, idGuide);
                                    }

                                    break;
                                }
                            }
                        }

                        
                    }

                    return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
                }
                else
                {
                    // Check if model responded some error
                    string msj = "No se pudo procesar la solicitud";
                    if(subscriptionsRepository.getLastError() != String.Empty)
                    {
                        msj = subscriptionsRepository.getLastError();
                    }
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, msj, null);
                }

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idClient"></param>
        /// <param name="idList"></param>
        /// <param name="idGuide"></param>
        /// <returns></returns>
        private ActionResponse CallExternal(int idClient, int idList, int idGuide)
        {

            ActionResponse external = null;

            var client = new RestClient(System.Configuration.ConfigurationManager.AppSettings["URL_SERVICE_UPDATE_LIST"]);
            var request = new RestRequest(Method.PUT);

            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-api-key", System.Configuration.ConfigurationManager.AppSettings["KEY_SERVICE_UPDATE_LIST"]);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\n\t\"idList\":"+ idList.ToString() + ",\n\t\"idClient\":"+ idClient.ToString() + ",\n\t\"idGuide\":" + idGuide.ToString() + "\n}",  ParameterType.RequestBody);
            var response = client.Execute(request);

            var content = response.Content;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Must to check structure, 200 doesn't mean OK (it could be error controlated)
                if(!content.Contains("error"))
                {
                    //JsonResponseOk json = JsonConvert.DeserializeObject<JsonResponseOk>(content);
                }
                else
                {
                    //JsonResponseError json = JsonConvert.DeserializeObject<JsonResponseError>(content);
                }
                
            }
            else
            {
                // Error
                //JsonResponseError json = JsonConvert.DeserializeObject<JsonResponseError>(content);
            }

            

            return external;
        }

        public ActionResponse SetProfileAction(int idProduct, dynamic bodyObj)
        {
            try
            {
                var checkDesc = profilesRepository.GetProfileByNameOrTagName(idProduct, bodyObj.Name);
                var checkTagName = profilesRepository.GetProfileByNameOrTagName(idProduct, bodyObj.TagName);

                /*if (checkDesc != null || checkTagName != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }*/

                var action = profilesRepository.NewRecord(idProduct, bodyObj);

                if (action != null)
                {
                    Profile p = new Profile
                    {
                        idProfile = action.IdProfile,
                        name = action.Name,
                        description = action.Description,
                        active = action.Active,
                        tagName = action.TagName,
                        paid = action.Paid
                    };

                    return utilities.Response((int)CodeStatusEnum.OK, "OK", p);
                }
                else
                {
                    string msj = "No se pudo procesar la solicitud";
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, msj, null);
                }

            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        public ActionResponse UpdateProfileAction(int idProfile, dynamic body)
        {
            try
            {
                var checkDesc = profilesRepository.GetProfileByNameOrTagName(body.idProduct, body.name);
                var checkTagName = profilesRepository.GetProfileByNameOrTagName(body.idProduct, body.tagName);

                /*if (checkDesc != null || checkTagName != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }*/

                var action = profilesRepository.UpdateRecord(idProfile, body);

                if (action != null)
                {
                    // Format response with RFC-3339
                    UpdateResponse response = new UpdateResponse
                    {
                        updated = DateTime.Now
                    };

                    return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
                }
                else
                {
                    string msg = "No se pudo procesar la solicitud";
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, msg, null);
                }

            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }
    }
}
