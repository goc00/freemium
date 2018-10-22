using Business.Libraries;
using Contract.Interfaces;
using Contract.Models.Dto;
using Contract.Models.Enum;
using Contract.Models.Response;
using Contract.Models.Response.Common;
using Repository.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Implementation
{
    public class DimensionsService : IDimensionsService
    {
        private ProductsRepository productsRepository;
        private UsersRepository usersRepository;
        private ProfilesDimensionsRepository profilesDimensionsRepository;
        private DimensionsRepository dimensionsRepository;
        private ProfilesRepository profilesRepository;
        private UsersDimensionsRepository usersDimensionsRepository;
        private SubscriptionsRepository subscriptionsRepository;

        private Utilities utilities;

        public DimensionsService()
        {
            this.productsRepository = new ProductsRepository();
            this.usersRepository = new UsersRepository();
            this.profilesDimensionsRepository = new ProfilesDimensionsRepository();
            this.dimensionsRepository = new DimensionsRepository();
            this.profilesRepository = new ProfilesRepository();
            this.usersDimensionsRepository = new UsersDimensionsRepository();
            this.subscriptionsRepository = new SubscriptionsRepository();

            this.utilities = new Utilities();
        }

        public ActionResponse GetDimensionsByProductAction(int idProduct)
        {
            try
            {
                var dimensions = dimensionsRepository.GetDimensionsByProduct(idProduct);

                if (dimensions == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado registros", null);
                }


                List<SimpleDimension> lst = new List<SimpleDimension>();

                foreach (var dimension in dimensions)
                {
                    SimpleDimension d = new SimpleDimension
                    {
                        idDimension = dimension.IdDimension,
                        nameDimension = dimension.Description,
                        tagDimension = dimension.TagName,
                        idDimensionType = dimension.IdDimensionType.Value,
                        idDimensionCategory = dimension.IdDimensionCategory.Value,
                        tagDimensionCategory = dimension.DimensionsCategories.TagName,
                        nameDimensionType = dimension.DimensionsTypes.Description,
                        idProduct = (int)dimension.DimensionsCategories.IdProduct
                    };


                    lst.Add(d);
                }

                GetDimensionListResponse response = new GetDimensionListResponse();
                response.totalItems = dimensions.Count;
                response.items = lst;

                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="dimensionData"></param>
        /// <returns></returns>
        public ActionResponse SetDimensionAction(int idProduct, dynamic dimensionData)
        {
            try
            {
                var checkDimension = dimensionsRepository.CheckValidDimension(
                    dimensionData.idDimensionType,
                    dimensionData.idDimensionCategory,
                    dimensionData.tagName
                );

                if (checkDimension != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }

                var action = dimensionsRepository.NewRecord(idProduct, dimensionData);

                if (action != null)
                {

                    SetDimensionResponse response = new SetDimensionResponse
                    {
                        dimension = new DimensionData
                        {
                            idDimension = action.IdDimension,
                            description = action.Description,
                            tagName = action.TagName,
                            idDimensionType = action.IdDimensionType,
                            idDimensionCategory = action.IdDimensionCategory,
                            value = action.Value,
                            switchValue = action.SwitchValue,
                            active = action.Active
                        }
                    };
                    return utilities.Response((int)CodeStatusEnum.OK, "OK", response.dimension);
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

        /// <summary>
        /// Update Record
        /// </summary>
        /// <param name="idDimension"></param>
        /// <param name="dimensionData"></param>
        /// <returns></returns>
        public ActionResponse UpdateDimensionAction(int idDimension, dynamic dimensionData)
        {
            try
            {
                var action = dimensionsRepository.UpdateRecord(idDimension, dimensionData);

                if (action != null)
                {
                    // Format response with RFC-3339
                    UpdateResponse response = new UpdateResponse();
                    response.updated = DateTime.Now;

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

        /// <summary>
        /// Get dimension's attributes. Service will check kind of dimension, then it will find its data associated
        /// for example: if dimension is consumible, need user ID to get subscription and check UserDimension
        /// </summary>
        /// <param name="idProduct">ID Product</param>
        /// <param name="idClient">ID Client (ID user master)</param>
        /// <param name="idProfile">ID profile</param>
        /// <param name="idDimension">ID dimension</param>
        /// <returns>ActionResponse object (200 OK - <> 200 NOK)</returns>
        public ActionResponse GetDimensionAction(int idProduct, int idClient, int idProfile, int idDimension)
        {
    
            try
            {

                // Check if product exists
                var oProduct = this.productsRepository.GetProduct(idProduct);
                if(oProduct == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el producto", null);
                }

                // Check if client (user) exists or it's linked to product
                var oUser = this.usersRepository.GetUserv2(idClient.ToString(), idProduct);
                if(oUser == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El usuario no existe o no está relacionado con el producto", null);
                }
                if(!oUser.Active.Value)
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "El usuario indicado no se encuentra activo en la plataforma", null);
                }
                int idUser = oUser.IdUser; // ID USER FREEMIUM

                // Looking for dimension
                var oDimension = this.dimensionsRepository.GetDimension(idDimension);
                if(oDimension == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe ninguna dimensión con el identificador proporcionado", null);
                }

                // Finding dimensioncategories (needed for get dimension)
                var dimensionCategories = oProduct.DimensionsCategories;
                if(dimensionCategories.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existen categorías de dimensión asociadas al producto", null);
                }

                bool okDimension = false;
                foreach(var oDimensionCategory in dimensionCategories)
                {
                    // Will find dimension
                    if(oDimensionCategory.IdDimensionCategory == oDimension.IdDimensionCategory)
                    {
                        okDimension = true;
                        break;
                    }
                }
                if(!okDimension)
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "La dimensión no está relacionada a ninguna categoría", null);
                }



                // Finding profile
                // Check if profile provided is part of product sent
                var profiles = oProduct.Profiles;
                if(profiles.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El producto no posee perfiles configurados", null);
                }

                foreach(var o in profiles)
                {
                    if(o.IdProfile == idProfile)
                    {

                        string msgProfileNonActive = "El perfil no se encuentra activo en el sistema";
                        if(o.Active == null)
                        {
                            return utilities.Response((int)CodeStatusEnum.CONFLICT, msgProfileNonActive, null);
                        }
                        else
                        {
                            if(!o.Active.Value)
                            {
                                return utilities.Response((int)CodeStatusEnum.CONFLICT, msgProfileNonActive, null);
                            }
                        }

                        // Profile is found, will be used it in function to dimension type

                        // Check dimension type:
                        // If dimension is numeric (static) or switch, only we need its value
                        // If dimension is consumible, we need to get more data from userdimension entity
                        Object value = null;
                        Object originalValue = null;
                        // ProfileDimensions
                        var profileDimension = this.profilesDimensionsRepository.GetProfileDimensionPD(idProfile, idDimension);
                        if(profileDimension == null)
                        {
                            return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar la relación del perfil con la dimensión", null);
                        }

                        switch(oDimension.IdDimensionType.Value)
                        {
                            case (int)DimensionTypeEnum.NUMERIC:
                                value = profileDimension.Value; // decimal
                                break;
                            case (int)DimensionTypeEnum.SWITCH:
                                value = profileDimension.SwitchValue; // bool
                                break;
                            case (int)DimensionTypeEnum.CONSUMIBLE:
                                // Subscription
                                var oSubscription = this.subscriptionsRepository.GetUserCurrentSubscription(idUser);
                                if(oSubscription == null)
                                {
                                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar la suscripción del cliente", null);
                                }
                                // UserDimension
                                var oUserDimension = this.usersDimensionsRepository.GetUserDimension(idDimension, oSubscription.IdSubscription);
                                if(oUserDimension == null)
                                {
                                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar el valor actual de la dimensión/perfil", null);
                                }
                                value = oUserDimension.CurrentValue;
                                originalValue = profileDimension.Value;
                                break;
                            default:
                                return utilities.Response((int)CodeStatusEnum.CONFLICT, "No se pudo determinar el tipo de dimensión", null);
                        }

                        
                       
                        GetDimensionResponse response = new GetDimensionResponse();
                        response.nameDimension = oDimension.Description;
                        response.tagName = oDimension.TagName;
                        response.idDimensionType = oDimension.IdDimensionType.Value;
                        DimensionTypeEnum dimensionTypeEnum = (DimensionTypeEnum)oDimension.IdDimensionType.Value;
                        response.nameDimensionType = dimensionTypeEnum.ToString();
                        response.currentValue = value;
                        response.originalValue = originalValue;

                        return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
 
                    }
                }

                // At this point, profile doesn't exist, then need to stop action
                return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe ningún perfil asociado con el producto", null);

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }


        /// <summary>
        /// Update dimension value, it will check dimension type for 
        /// </summary>
        /// <param name="idProduct">ID Product</param>
        /// <param name="idClient">ID Client (external code)</param>
        /// <param name="idProfile">ID Profile</param>
        /// <param name="idDimension">ID Dimension (must to be consumible)</param>
        /// <param name="value">New value for dimension</param>
        /// <returns></returns>
        public ActionResponse UpdateValueDimensionAction(int idProduct, int idClient, int idProfile, int idDimension, decimal value)
        {
            try
            {

                // Check if product exists
                var oProduct = this.productsRepository.GetProduct(idProduct);
                if(oProduct == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el producto", null);
                }

                // Check if client (user) exists or it's linked to product
                var oUser = this.usersRepository.GetUserv2(idClient.ToString(), idProduct);
                if(oUser == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El usuario no existe o no está relacionado con el producto", null);
                }
                if(!oUser.Active.Value)
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "El usuario indicado no se encuentra activo en la plataforma", null);
                }
                int idUser = oUser.IdUser; // ID USER FREEMIUM

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

                // Check if dimension exists
                var oDimension = this.dimensionsRepository.GetDimension(idDimension);
                if(oDimension == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe ninguna dimensión con el identificador proporcionado", null);
                }

                // Check dimension is related to profile across dimensioncategories
                var dimensionCategories = oProduct.DimensionsCategories;
                if(dimensionCategories.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existen categorías de dimensión asociadas al producto", null);
                }

                bool okDimension = false;
                foreach(var oDimensionCategory in dimensionCategories)
                {
                    // Will find dimension
                    if(oDimensionCategory.IdDimensionCategory == oDimension.IdDimensionCategory)
                    {
                        okDimension = true;
                        break;
                    }
                }
                if(!okDimension)
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "La dimensión no está relacionada a ninguna categoría", null);
                }


                // Update value dimension
                // If dimension is consumible will update it
                if(oDimension.IdDimensionType == (int)DimensionTypeEnum.CONSUMIBLE)
                {
                    // Find subscription
                    var subscription = subscriptionsRepository.GetUserCurrentSubscription(idUser);
                    if(subscription == null)
                    {
                        return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar la subscripción del usuario", null);
                    }

                    // Update value
                    var userDimension = usersDimensionsRepository.GetUserDimension(idDimension, subscription.IdSubscription);
                    if(userDimension == null)
                    {
                        return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar la dimensión del usuario", null);
                    }
                    if(userDimension.CurrentValue == null)
                    {
                        return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se pudo determinar el valor de la dimensión del usuario", null);
                    }

                    decimal oldValueX = userDimension.CurrentValue.Value;

                    bool action = usersDimensionsRepository.UpdateUserDimensionValue(userDimension.IdUserDimension, value);

                    DimensionTypeEnum dimensionTypeEnum = (DimensionTypeEnum)oDimension.IdDimensionType.Value;
                    UpdateValueDimensionResponse response = new UpdateValueDimensionResponse
                    {
                        idDimension = idDimension,
                        nameDimension = oDimension.Description,
                        tagName = oDimension.TagName,
                        idDimensionType = oDimension.IdDimensionType.Value,
                        nameDimensionType = dimensionTypeEnum.ToString(),
                        oldValue = oldValueX,
                        currentValue = value
                    };


                    return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
                }
                else
                {
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "La dimensión no es del tipo consumible", null);
                }


                

                

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        // -------------------------------------------------- DEPRECATED LOGIC --------------------------------------------------



        public decimal GetNumericDimension(string ProductToken, string UserCode, string DimensionTag)
        {

            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(ProductToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + ProductToken, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(idProduct, DimensionTag);
            if (dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión " + DimensionTag + "(" + dimension.IdDimension + ") no existe para este producto", null);
            }

            //obtiene datos del usuario
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
            {
                //throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);

                //si el usuario no existe, le retorna el valor del perfil anonimo para el producto
                return GetAnonNumericDimension(idProduct, dimension.IdDimension);
            }

            if (user.Active == false)
            {
                throw new Contract.Exceptions.UserInactiveException("El usuario indicado no se encuentra Activo en la plataforma", null);
            }

            //ahora, al ser de tipo switch, ve directamente el valor para el perfil del usuario y lo retorna
            var profiledimension = profilesDimensionsRepository.GetProfileDimension(user.IdUser, dimension.IdDimension);

            if (profiledimension == null)
            {
                //si la dimensión no está registrada en el perfil, arroja el valor por defecto de la dimension

                if (dimension.Value == null)
                {
                    throw new Contract.Exceptions.ProfileDimensionNoDefaultValueException("La dimensión " + DimensionTag + "(" + dimension.IdDimension + ") no tiene un valor por defecto.", null);
                }

                return (decimal)dimension.Value;

                //throw new Contract.Exceptions.ProfileDimensionNotConfiguredException("La dimensión " + DimensionTag + " no está bien configurada en todos los perfiles. No se encontro una relación entre el perfil actual del usuario y la dimensión.", null);
            }

            if (profiledimension.Dimensions.DimensionsTypes == null)
            {
                throw new Contract.Exceptions.DimensionTypeNotConfiguredSwitchException("La dimension " + DimensionTag + " No tiene tipo (Switch/Numeric/NumericConsumable) ", null);
            }

            if (!profiledimension.Dimensions.DimensionsTypes.TagName.Equals("Numeric"))
            {
                throw new Contract.Exceptions.DimensionTypeIsNotSwitchException("La dimension " + DimensionTag + " NO es de tipo Numérica, es de tipo " + profiledimension.Dimensions.DimensionsTypes.TagName, null);
            }

            //retorno
            return (decimal)profiledimension.Value;
        }

        public decimal GetAnonNumericDimension(int IdProduct, int IdDimension)
        {
            //obtiene al producto
            var product = productsRepository.GetProduct(IdProduct);
            if (product == null)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el id " + IdProduct, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(IdDimension);
            if (dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión (" + IdDimension + ") no existe para este producto", null);
            }

            //ahora va a buscar el perfil anonimo por defecto del producto
            var profile = profilesRepository.GetProfileByAnon(IdProduct);

            var profiledimension = profile.ProfilesDimensions.FirstOrDefault(e=>e.IdDimension == IdDimension);

            //validar existencia de dimension del perfil anonimo buscada
            if (profiledimension == null)
            {
                throw new Contract.Exceptions.ProfileDimensionAnonNoConfiguredException("El producto (" + IdProduct + ") no tiene configurado ninguna relacion para usuario ANONIMO con la dimension (" + IdDimension + ")", null);
            }

            if (profiledimension.Value == null)
            {
                throw new Contract.Exceptions.ProfileDimensionAnonNoConfiguredException("La profiledimension(" + profiledimension.IdProfileDimension + ") no tiene ningún valor SWITCH por defecto", null);

            }

            //ahora va a buscar la dimension y su valor por defecto para ese perfil
            return (decimal)profiledimension.Value;
        }

        public bool GetAnonSwitchDimension(int IdProduct, int IdDimension)
        {
            //obtiene al producto
            var product = productsRepository.GetProduct(IdProduct);
            if (product == null)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el id " + IdProduct, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(IdDimension);
            if (dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión ("+ IdDimension + ") no existe para este producto", null);
            }

            //ahora va a buscar el perfil anonimo por defecto del producto
            var profile = profilesRepository.GetProfileByAnon(IdProduct);

            var profiledimension = profile.ProfilesDimensions.FirstOrDefault(e => e.IdDimension == IdDimension);

            //validar existencia de dimension del perfil anonimo buscada
            if (profiledimension == null)
            {
                throw new Contract.Exceptions.ProfileDimensionAnonNoConfiguredException("El producto ("+IdProduct+") no tiene configurado ninguna relacion para usuario ANONIMO con la dimension ("+IdDimension+")", null);
            }

            if (profiledimension.SwitchValue == null)
            {
                throw new Contract.Exceptions.ProfileDimensionAnonNoConfiguredException("La profiledimension("+ profiledimension.IdProfileDimension + ") no tiene ningún valor SWITCH por defecto", null);

            }

            //ahora va a buscar la dimension y su valor por defecto para ese perfil
            return (bool)profiledimension.SwitchValue;

        }

        public bool GetSwitchDimension(string ProductToken, string UserCode, string DimensionTag)
        {
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(ProductToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + ProductToken, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(idProduct, DimensionTag);
            if(dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión "+ DimensionTag + "(" + dimension.IdDimension + ") no existe para este producto", null);
            }

            //obtiene datos del usuario
            var user = usersRepository.GetUser(UserCode,idProduct);
            if (user == null)
            {
                //throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);

                //si el usuario no existe, le retorna el valor del perfil anonimo para el producto
                return GetAnonSwitchDimension(idProduct,dimension.IdDimension);
            }

            if (user.Active == false)
            {
                throw new Contract.Exceptions.UserInactiveException("El usuario indicado no se encuentra Activo en la plataforma", null);
            }

            //ahora, al ser de tipo switch, ve directamente el valor para el perfil del usuario y lo retorna
            var profiledimension = profilesDimensionsRepository.GetProfileDimension(user.IdUser, dimension.IdDimension);

            if (profiledimension == null)
            {
                //si la dimensión no está registrada en el perfil, arroja el valor por defecto de la dimension

                if (dimension.SwitchValue == null)
                {
                    throw new Contract.Exceptions.ProfileDimensionNoDefaultValueException("La dimensión " + DimensionTag + "("+ dimension.IdDimension + ") no tiene un valor por defecto.", null);
                }

                return (bool)dimension.SwitchValue;

                //throw new Contract.Exceptions.ProfileDimensionNotConfiguredException("La dimensión " + DimensionTag + " no está bien configurada en todos los perfiles. No se encontro una relación entre el perfil actual del usuario y la dimensión.", null);
            }

            if (profiledimension.Dimensions.DimensionsTypes == null)
            {
                throw new Contract.Exceptions.DimensionTypeNotConfiguredSwitchException("La dimension " + DimensionTag + " No tiene tipo (Switch/Numeric/NumericConsumable) ", null);
            }

            if (!profiledimension.Dimensions.DimensionsTypes.TagName.Equals("Switch"))
            {
                throw new Contract.Exceptions.DimensionTypeIsNotSwitchException("La dimension " + DimensionTag + " NO es de tipo Switch, es de tipo "+ profiledimension.Dimensions.TagName, null);
            }

            //retorno
            return (bool)profiledimension.SwitchValue;
        }

        /**********************************************************************************/

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public decimal GetNumericConsumableDimension(string ProductToken, string UserCode, string DimensionTag)
        {
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(ProductToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + ProductToken, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(idProduct, DimensionTag);
            if (dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión " + DimensionTag + "(" + dimension.IdDimension + ") no existe para este producto", null);
            }

            //obtiene datos del usuario
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
            {
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);
            }

            if (user.Active == false)
            {
                throw new Contract.Exceptions.UserInactiveException("El usuario indicado no se encuentra Activo en la plataforma", null);
            }

            //obtengo la suscripcion
            //obtiene la suscripcion
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario " + UserCode + " del producto " + ProductToken + " no tiene suscripcion", "Sin suscripcion");
                throw new Contract.Exceptions.SubscriptionNotFoundException("No se encontró una subscripcion para el UserCode " + UserCode, null);
            }

            if (sub.IdProfile == 0)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario " + UserCode + " del producto " + ProductToken + " no tiene perfil en su suscripcion", "Sin perfil en susc.");
                throw new Contract.Exceptions.ProfileNotFoundException("No se encontró un perfil configurado para la suscripcion del usuario", null);
            }


            //obtengo el userdimension
            var userdimension = usersDimensionsRepository.GetUserDimension(dimension.IdDimension, sub.IdSubscription);

            if (userdimension == null)
                throw new Contract.Exceptions.UserDimensionNotFoundException("Usuario no tiene configurada la dimension", null);

            //TODO OK

            return (decimal)userdimension.CurrentValue;

        }

        decimal IDimensionsService.ConsumeNumericDimension(string ProductToken, string UserCode, string DimensionTag, decimal amount)
        {
            throw new NotImplementedException();
        }

        public decimal ConsumeNumericConsumableDimension(string ProductToken, string UserCode, string DimensionTag, decimal Amount)
        {
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(ProductToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + ProductToken, null);

            //validar existencia de dimension buscada
            Repository.EntityFramework.Dimensions dimension = dimensionsRepository.GetDimension(idProduct, DimensionTag);
            if (dimension == null)
            {
                throw new Contract.Exceptions.ProductDimensionNotFoundException("La dimensión " + DimensionTag + "(" + dimension.IdDimension + ") no existe para este producto", null);
            }

            //obtiene datos del usuario
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
            {
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);
            }

            if (user.Active == false)
            {
                throw new Contract.Exceptions.UserInactiveException("El usuario indicado no se encuentra Activo en la plataforma", null);
            }

            //obtengo la suscripcion
            //obtiene la suscripcion
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario " + UserCode + " del producto " + ProductToken + " no tiene suscripcion", "Sin suscripcion");
                throw new Contract.Exceptions.SubscriptionNotFoundException("No se encontró una subscripcion para el UserCode " + UserCode, null);
            }

            if (sub.IdProfile == 0)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario " + UserCode + " del producto " + ProductToken + " no tiene perfil en su suscripcion", "Sin perfil en susc.");
                throw new Contract.Exceptions.ProfileNotFoundException("No se encontró un perfil configurado para la suscripcion del usuario", null);
            }

            //obtengo el profile dimension
            var profiledimension = this.profilesDimensionsRepository.GetProfileDimension(user.IdUser, dimension.IdDimension);
            if (profiledimension == null)
            {
                throw new Contract.Exceptions.ProfileDimensionNotConfiguredException("La dimension no fue configurada con el perfil", null);
            }


            if (profiledimension.IsInfinite == true)
            {
                //si la dimension es infinita, solo devuelve el valor que le pasaron como parametro
                //var userdimension = usersDimensionsRepository.GetUserDimension(dimension.IdDimension, sub.IdSubscription);

                //return (decimal)userdimension.CurrentValue;
                return Amount;
            }
            else
            {
                //si la dimension NO ES FININITA consume y devuelve valor
                /*
                var userdimension = usersDimensionsRepository.ConsumeAndGetUserDimension(dimension.IdDimension, sub.IdSubscription, Amount);

                if (userdimension == null)
                    throw new Contract.Exceptions.UserDimensionNotFoundException("Usuario no tiene configurada la dimension", null);
                    */

                return usersDimensionsRepository.ConsumeAndGetConsumedUserDimensionValue(dimension.IdDimension, sub.IdSubscription, Amount);
            }
        }
    }
}
