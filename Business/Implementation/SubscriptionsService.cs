using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.Interfaces;
using Contract.Models;
using Repository.Implementation;
using System.Net;
using System.Net.Http;
using Contract.Models.Response.Common;
using Business.Libraries;
using Contract.Models.Enum;
using Contract.Models.Response;
using Contract.Models.Dto;

namespace Business.Implementation
{
    public class SubscriptionsService : ISubscriptionsService
    {

        private ProductsRepository productsRepository { get; set; }
        private UsersRepository usersRepository { get; set; }
        private SubscriptionsRepository subscriptionsRepository { get; set; }
        private ProfilesRepository profilesRepository { get; set; }
        private ProfilesDimensionsRepository profilesDimensionsRepository { get; set; }
        private PromosRepository promosRepository { get; set; }
        private DimensionsRepository dimensionsRepository { get; set; }
        private UsersDimensionsRepository usersDimensionsRepository { get; set; }

        private Utilities utilities;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public SubscriptionsService()
        {
            this.productsRepository = new ProductsRepository();
            this.usersRepository = new UsersRepository();
            this.subscriptionsRepository = new SubscriptionsRepository();
            this.profilesRepository = new ProfilesRepository();
            this.profilesDimensionsRepository = new ProfilesDimensionsRepository();
            this.promosRepository = new PromosRepository();
            this.dimensionsRepository = new DimensionsRepository();
            this.usersDimensionsRepository = new UsersDimensionsRepository();

            this.utilities = new Utilities();
        }

        /// <summary>
        /// Will reset all CONSUMIBLE dimensions related to user and product
        /// </summary>
        /// <param name="idProduct">ID product</param>
        /// <param name="idClient">ID client (User master ID)</param>
        /// <returns></returns>
        public ActionResponse ResetSubscriptionAction(int idProduct, int idClient)
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
                if(oUser == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el usuario en el sistema", null);
                }
                int idUser = oUser.IdUser; // ID USER FREEMIUM

                // Find subscription
                var oSubscription = this.subscriptionsRepository.GetUserCurrentSubscription(idUser);
                if(oSubscription == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se ha detectado ninguna suscripción para el usuario", null);
                }
                // If subscription is found, we need to check if has profile setted at least
                if(oSubscription.IdProfile == null || oSubscription.IdProfile == 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La suscripción no está asociada a ningún perfil", null);
                }
                int idProfile = oSubscription.IdProfile.Value;

                // Check if profile exists
                var oProfile = profilesRepository.GetProfile(idProfile);
                if(oProfile == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "El perfil vinculado a la suscripción no existe en el sistema", null);
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

                // Get dimensions by profile
                var dimensions = dimensionsRepository.GetProfileDimensions(idProfile);

                // Travel dimensions, if type is consumible, will get user dimensions vinculated
                int l = dimensions.Count;
                int updated = 0;
                if(l > 0)
                {

                    foreach(var item in dimensions) // item = dimension object (from repository)
                    {

                        int idDimension = item.IdDimension;

                        // ProfileDimension associated
                        var profileDimension = profilesDimensionsRepository.GetProfileDimensionPD(idProfile, idDimension);
                        if(profileDimension == null)
                        {
                            return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se se pudo obtener la relación del perfil con la dimensión", null);
                        }

                        if((int)item.IdDimensionType == (int)DimensionTypeEnum.CONSUMIBLE)
                        {
                            // Get UserDimension
                            var userDimension = usersDimensionsRepository.GetUserDimension(idDimension, oSubscription.IdSubscription);
                            if(userDimension == null)
                            {
                                return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se se pudo obtener la relación de la dimensión con el usuario", null);
                            }

                            // Reset value
                            bool action = this.usersDimensionsRepository.UpdateUserDimensionValue(userDimension.IdUserDimension, profileDimension.Value.Value);
                            if(action) { updated++; }

                        }
   
                    }

                }

                ResetSubscriptionResponse response = new ResetSubscriptionResponse();
                response.updated = DateTime.Now;
                response.totalItemsUpdated = updated;
                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }

        }


        /// <summary>
        /// Get all data related to user (profiles + dimensions)
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="idClient"></param>
        /// <returns></returns>
        public ActionResponse GetDataUserAction(int idProduct, int idClient)
        {

            try
            {
                // Check for product
                var oProduct = productsRepository.GetProduct(idProduct);
                if(oProduct == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No existe el producto", null);
                }

                // Get user (will check against idUserExternal)
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

                // Get subscription data
                var subscription = subscriptionsRepository.GetUserCurrentSubscription(idUser);

                if(subscription == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se ha podido determinar la suscripción del usuario", null);
                }

                // Profile about subscription
                var profile = profilesRepository.GetProfile(subscription.IdProfile.Value);
                if(profile == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se ha podido identificar el perfil asociado al usuario", null);
                }

                int idProfile = profile.IdProfile;

                // Get dimensions by profile
                var dimensions = dimensionsRepository.GetProfileDimensions(idProfile);

                // Travel dimensions, if type is consumible, will get user dimensions vinculated
                List<Dimension> newDimensions = new List<Dimension>();
                if(dimensions.Count > 0)
                {

                    foreach(var item in dimensions) // item = dimension object (from repository)
                    {

                        Dimension dimensionOut = new Dimension()
                        {
                            idDimension = (int)item.IdDimension,
                            idDimensionType = item.IdDimensionType.Value,
                            idDimensionCategory = item.DimensionsCategories.IdDimensionCategory,
                            nameDimension = item.Description,
                            tagDimension = item.TagName,
                            tagDimensionCategory = item.DimensionsCategories.TagName
                        };

                        // ProfileDimension associated
                        var profileDimension = profilesDimensionsRepository.GetProfileDimensionPD(idProfile, dimensionOut.idDimension);
                        if(profileDimension == null)
                        {
                            return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se se pudo obtener la relación del perfil con la dimensión", null);
                        }

                        switch((int)item.IdDimensionType)
                        {
                            case (int)DimensionTypeEnum.NUMERIC:
                            case (int)DimensionTypeEnum.SWITCH:

                                if((int)item.IdDimensionType == (int)DimensionTypeEnum.NUMERIC)
                                {
                                    if(profileDimension.Value == null)
                                    {
                                        return utilities.Response((int)CodeStatusEnum.CONFLICT, "No se ha detectado ningún valor para el atributo 'Value' en Dimensión " + dimensionOut.idDimension + ", por favor corregir", null);
                                    }
                                    dimensionOut.currentValue = profileDimension.Value.Value;
                                } else
                                {
                                    if(profileDimension.SwitchValue == null)
                                    {
                                        return utilities.Response((int)CodeStatusEnum.CONFLICT, "No se ha detectado ningún valor para el atributo 'SwitchValue' en Dimensión " + dimensionOut.idDimension + ", por favor corregir", null);
                                    }
                                    dimensionOut.currentValue = profileDimension.SwitchValue.Value;
                                }

                                break;
                            case (int)DimensionTypeEnum.CONSUMIBLE:

                                // Get UserDimension
                                var userDimension = usersDimensionsRepository.GetUserDimension(item.IdDimension, subscription.IdSubscription);
                                bool valSetWithNewUD = false;
                                if(userDimension == null)
                                {
                                    // If user dimension is not set, we will create it in order to normalize user configuration
                                    var newDimension = usersDimensionsRepository.NewUserDimension(item.IdDimension, subscription.IdSubscription);
                                    if(newDimension.CurrentValue == null)
                                    {
                                        return utilities.Response((int)CodeStatusEnum.CONFLICT, "Imposible determinar 'CurrentValue' para Dimensión " + item.IdDimension + " y Suscripción " + subscription.IdSubscription + ", por favor corregir", null);
                                    }
                                    dimensionOut.currentValue = newDimension.CurrentValue.Value;
                                    valSetWithNewUD = true;

                                    //return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se se pudo obtener la relación de la dimensión con el usuario", null);
                                }

                                if(!valSetWithNewUD)
                                {
                                    if(userDimension.CurrentValue == null)
                                    {
                                        return utilities.Response((int)CodeStatusEnum.CONFLICT, "Imposible determinar 'CurrentValue' para Dimensión " + item.IdDimension + " y Suscripción " + subscription.IdSubscription + ", por favor corregir", null);
                                    }
                                    dimensionOut.currentValue = userDimension.CurrentValue.Value;
                                }
                                if(profileDimension.Value == null)
                                {
                                    return utilities.Response((int)CodeStatusEnum.CONFLICT, "No se ha detectado ningún valor para el atributo 'Value' en Dimensión " + dimensionOut.idDimension + ", por favor corregir", null);
                                }
                                dimensionOut.originalValue = profileDimension.Value.Value;

                                break;
                        }

                        DimensionTypeEnum dimensionTypeEnum = (DimensionTypeEnum)dimensionOut.idDimensionType;
                        dimensionOut.nameDimensionType = dimensionTypeEnum.ToString();

                        // Add object
                        newDimensions.Add(dimensionOut);
                    }

                }

                // Response object
                GetDataUserResponse response = new GetDataUserResponse
                {
                    idSubscription = subscription.IdSubscription,
                    dateCreated = subscription.DateCreated.Value,
                    profile = new Profile
                    {
                        idProfile = profile.IdProfile,
                        name = profile.Name,
                        description = profile.Description,
                        tagName = profile.TagName,
                        active = profile.Active,
                        paid = profile.Paid
                    },
                    dimensions = newDimensions
                };


                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);

            }
            catch(Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }

        }



        // -------------------------------------------------- DEPRECATED LOGIC --------------------------------------------------


        public Subscriptions Get(string ProductToken, string UserCode)
        {
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(ProductToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken "+ProductToken, null);

            //obtiene datos del usuario
            var user = usersRepository.GetUser(UserCode,idProduct);
            if (user == null)
            {
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);
            }

            if (user.Active == false)
            {
                throw new Contract.Exceptions.UserInactiveException("El usuario indicado no se encuentra Activo en la plataforma",null);
            }

            //obtiene datos del perfil del usuario respecto al producto
            var subscription = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            //perfil segun suscripcion
            Repository.EntityFramework.Profiles profile;

            if (subscription == null)
            {
                //si no tiene suscripcion, le crea una en el estándar de suscrito sin suscripción.
                profile = profilesRepository.GetStandardFreeSuscription(user.IdUser);

                if (profile == null)
                    throw new Contract.Exceptions.StandardProfileNotFoundException("El producto no tiene un perfil estándar para cuando el usuario está registrado pero no está suscrito a ningún perfil. Usuario: " + UserCode, null);

                //setea suscripción vacía 
                subscription = new Repository.EntityFramework.Subscriptions();
            }
            else
            {
                profile = subscription.Profiles;

                if (profile == null) {
                    //si no tiene suscripcion, le crea una en el estándar de suscrito sin suscripción.
                    profile = profilesRepository.GetStandardFreeSuscription(user.IdUser);

                    if (profile == null)
                        throw new Contract.Exceptions.StandardProfileNotFoundException("El producto no tiene un perfil estándar para cuando el usuario está registrado pero no está suscrito a ningún perfil. Usuario: " + UserCode , null);
                }
            }

            List<UsersDimensions> dimensiones = new List<UsersDimensions>();

            foreach (var item in subscription.UsersDimensions)
            {
                dimensiones.Add(new UsersDimensions() {
                    IdDimension = (int)item.IdDimension,
                    Dimension = item.Dimensions.Description,
                    CurrentValue = (decimal)item.CurrentValue,
                    TagName = item.Dimensions.TagName
                });
            }
            
            //genera objeto de salida
            return new Subscriptions
            {
                IdSubscription  = subscription.IdSubscription,
                ExternalCode = subscription.ExternalCode,
                DateCreated = subscription.DateCreated,
                RenewalDay = subscription.RenewalDay,
                LastRenewal = subscription.LastRenewal,
                //IdUser = subscription.IdUser, //no debe ir dentro de lo que se expone como servicio. es de uso interno. Se utiliza el user de abajo como objeto
                IsCurrent = subscription.IsCurrent,

                Users = new Users
                {
                    Active = user.Active,
                    Email = user.Email,
                    ExternalCode = user.ExternalCode
                },

                Profiles = new Profiles
                {
                    ProfileId = profile.IdProfile,
                    Active = profile.Active,
                    Description = profile.Description,
                    Name = profile.Name,
                    TagName = profile.TagName,
                    Paid = profile.Paid
                },

                PromoFreeDays = subscription.PromoFreeDays,

                PromoStarted = subscription.PromoStarted,

                PromoEnd = subscription.PromoEnd,

                PromoActive = subscription.PromoActive,

                IdPromo = subscription.IdPromo,

                Dimensions = dimensiones
            };

        }

        //this is used for billing controller
        public bool SetSubscription(string UserCode, int IdPlan, string AppToken)
        {
            //verificar existencia de la app token
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(AppToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + AppToken, null);

            //obtener usuario segun el app token y external code
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);

            //verificar si existe una suscripcion actual. Si existe se cambia el IDPERFIL. 
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                //Si no existe, se crea la suscripcion.

                subscriptionsRepository.NewSubscription(new Repository.EntityFramework.Subscriptions
                {
                    Active = true,
                    DateCreated = DateTime.Now,
                    ExternalCode = "",
                    IdProfile = IdPlan,
                    IdUser = user.IdUser,
                    IsCurrent = true,
                    RenewalDay = DateTime.Now.Day,
                    LastRenewal = DateTime.Now
                });
            }
            else {
                subscriptionsRepository.SetSubscriptionProfile(sub.IdSubscription, IdPlan);
            }

            (new Repository.Implementation.EventLogRepository()).SetLog("POR PINCHAR ANALYTICS","GA");

            try
            {
                //avisa a google analytics si es que tiene el codigo
                var producto = productsRepository.GetProduct(idProduct);

                if (producto != null)
                {
                    (new Repository.Implementation.EventLogRepository()).SetLog("EXISTE PRODUCTO "+producto.IdProduct, "GA");
                    (new Repository.Implementation.EventLogRepository()).SetLog("CODIGO ANALYTICS " + producto.CodeAnalytics, "GA");
                    //si existe el codigo para el producto
                    if (!String.IsNullOrEmpty(producto.CodeAnalytics))
                    {
                        (new Repository.Implementation.EventLogRepository()).SetLog("TIENE CODIGO ANALYTICS " + producto.CodeAnalytics, "GA");
                        var profile = profilesRepository.GetProfile(IdPlan);
                        if (profile != null)
                        {
                            (new Repository.Implementation.EventLogRepository()).SetLog("TIENE PROFILE " + IdPlan + " (" + profile.Name + ")", "GA");
                            GoogleAnalyticsHelper.TrackEvent(producto.CodeAnalytics, @"usuario", @"registro", @"perfil", @"1");
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                //nothing
            }

            return true;
        }

        public bool SetSubscription(string UserCode, int IdPlan, string AppToken, int addDays)
        {
            //verificar existencia de la app token
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(AppToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + AppToken, null);

            //obtener usuario segun el app token y external code
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);

            //verificar si existe una suscripcion actual. Si existe se cambia el IDPERFIL. 
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                //Si no existe, se crea la suscripcion.

                subscriptionsRepository.NewSubscription(new Repository.EntityFramework.Subscriptions
                {
                    Active = true,
                    DateCreated = DateTime.Now,
                    ExternalCode = "",
                    IdProfile = IdPlan,
                    IdUser = user.IdUser,
                    IsCurrent = true,
                    RenewalDay = DateTime.Now.Day,
                    LastRenewal = DateTime.Now.AddDays(addDays)
                });
            }
            else
            {
                //si existe, se setea el nuevo perfily se asigna el tiempo
                subscriptionsRepository.SetSubscriptionProfileWithDays(sub.IdSubscription, IdPlan, addDays);
            }

            return true;
        }

        public bool SetSubscriptionWithPromo(string UserCode, int IdPlan, string AppToken, string TagPromo)
        {
            //verificar existencia de la app token
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(AppToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + AppToken, null);

            //obtener usuario segun el app token y external code
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);

            //voy a buscar la promo
            var promo = this.promosRepository.GetPromo(idProduct, TagPromo);

            //si el tag es nulo o vacio, elimino
            if (String.IsNullOrEmpty(TagPromo))
                promo = null;

            //por si esta mal configurada la promo
            if (promo != null && promo.FreeDays == null)
                promo.FreeDays = 0;

            //voy a buscar el profile por defecto
            var profile = this.profilesRepository.GetStandardFreeSuscription(user.IdUser);
            if (profile != null)
            {
                IdPlan = profile.IdProfile;
            }

            /*
            if (promo == null) {
                //throw new Contract.Exceptions.PromoNotFoundException("No se encontró promocion con Tag " + TagPromo, null);
                //genero promo en blanco para que peuda leer más adelante
                

            }*/

            //verificar si existe una suscripcion actual. Si existe se cambia el IDPERFIL. 
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                //Si no existe, se crea la suscripcion.
                Repository.EntityFramework.Subscriptions subs = new Repository.EntityFramework.Subscriptions
                {
                    Active = true,
                    DateCreated = DateTime.Now,
                    ExternalCode = "",
                    IdProfile = IdPlan,
                    IdUser = user.IdUser,
                    IsCurrent = true,
                    RenewalDay = DateTime.Now.Day
                    //LastRenewal = DateTime.Now.AddDays(addDays)
                };

                //si hay promo, le seteo los datos.
                if (promo != null)
                {
                    subs.IdProfile = promo.IdProfileActivePromo; //le setea el perfil que debe ir con la promo, independiente del que se envia por el servicio
                    subs.PromoActive = true;
                    subs.PromoStarted = DateTime.Now;
                    subs.PromoEnd = DateTime.Now.AddDays((int)promo.FreeDays);
                    subs.PromoFreeDays = promo.FreeDays;
                    subs.IdPromo = promo.IdPromo;
                }

                //guardo la nueva subscripcion
                subscriptionsRepository.NewSubscription(subs);
            }
            else
            {
                //si existe, se setea el nuevo perfil y se asigna la promo
                subscriptionsRepository.SetSubscriptionProfileWithPromo(sub.IdSubscription, IdPlan, promo);
            }

            return true;
        }

        public List<ActiveSubscriptionsData> GetActiveSubscriptions(string ProductToken)
        {
            return subscriptionsRepository.GetActiveSubscriptions(ProductToken);
        }

        public bool ResetSubscription(string AppToken, string UserCode)
        {
            //llama con categoria vacia
            return ResetSubscriptionByCategory(AppToken, UserCode, null);
        }

        public bool ResetSubscriptionByCategory(string AppToken, string UserCode, string Category)
        {
            //verificar existencia de la app token
            //obtiene ID de producto
            int idProduct = productsRepository.GetProductId(AppToken);
            if (idProduct == 0)
                throw new Contract.Exceptions.ProductsNotFoundException("No se encontró un producto con el ProductToken " + AppToken, null);

            //obtener usuario segun el app token y external code
            var user = usersRepository.GetUser(UserCode, idProduct);
            if (user == null)
                throw new Contract.Exceptions.UserNotFoundException("No se encontró un usuario con el UserCode " + UserCode, null);


            //obtiene la suscripcion
            var sub = subscriptionsRepository.GetUserCurrentSubscription(user.IdUser);

            if (sub == null)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario "+ UserCode + " del producto "+ AppToken + " no tiene suscripcion", "Sin suscripcion");
                return false;
            }

            //verifica si tiene perfil
            if (sub.IdProfile == null || sub.IdProfile == 0)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("El usuario " + UserCode + " del producto " + AppToken + " no tiene tiene configurado un perfil a su suscripcion", "Sin suscripcion");
                return false;
            }

            /////////////
            // todo ok //
            /////////////

            //obtengo el perfil
            var profile = profilesRepository.GetProfile((int)sub.IdProfile);

            if (profile == null)
            {
                (new Repository.Implementation.EventLogRepository()).SetLog("PERFIL ["+ (int)sub.IdProfile + "] NO EXISTE", "Perfil no existente");
                return true;
            }

            //hasta este punto el perfil existe. Se obtienen las dimensiones del perfil
            var dimensiones = dimensionsRepository.GetProfileDimensionsByCategory(profile.IdProfile, Category);

            foreach (var item in dimensiones)
            {
                //chequea si es de tipo consumible
                if (item.IdDimensionType == 3)
                {
                    //si es consumible, verifica que tenga su configuracion para el usuario, si no, la crea
                    var userdimension = item.UsersDimensions.FirstOrDefault(e=>e.IdDimension == item.IdDimension && e.IdSubscription == sub.IdSubscription);

                    if (userdimension == null)
                    {
                        //crea user dimension
                        usersDimensionsRepository.NewUserDimension(item.IdDimension, sub.IdSubscription);
                    }
                    else
                    {
                        usersDimensionsRepository.RestoreUserDimension(userdimension.IdUserDimension);
                    }
                }
            }

            return true;
        }

        public List<SubscriptionsResponse> GetSubscriptionsByRenewal(DateTime CurrentDate, int top)
        {
            var Subscriptions = subscriptionsRepository.GetSubscriptionsByRenewal(CurrentDate, top);

            List<SubscriptionsResponse> response = new List<SubscriptionsResponse>();

            foreach (var item in Subscriptions)
            {
                response.Add(new SubscriptionsResponse {
                    IdSubscription = item.IdSubscription,
                    ExternalCode = item.ExternalCode,
                    DateCreated = item.DateCreated,
                    RenewalDay = item.RenewalDay,
                    LastRenewal = item.LastRenewal,
                    IdUser = item.IdUser,
                    Active = item.Active,
                    IdProfile = item.IdProfile,
                    IsCurrent = item.IsCurrent,
                    PromoFreeDays = item.PromoFreeDays,
                    PromoStarted = item.PromoStarted,
                    PromoEnd = item.PromoEnd,
                    PromoActive = item.PromoActive,
                    IdPromo = item.IdPromo
                });
            }

            return response;
        }
    }
}
