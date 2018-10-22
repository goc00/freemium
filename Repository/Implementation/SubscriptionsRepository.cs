using Contract.Models;
using Contract.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class SubscriptionsRepository
    {
        public EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        private string lastError = "";

        /// <summary>
        /// Get last error marked by methods
        /// </summary>
        /// <returns>string</returns>
        public string getLastError()
        {
            return this.lastError;
        }

        /// <summary>
        /// Will set or update all related to change one profile. Need to be gobernated by transaction
        /// in order to keep consistency. Whatever profile is free or premium, requires create subscription
        /// Subscription links profile with user
        /// </summary>
        /// <param name="idProfile">ID new profile to be setted</param>
        /// <param name="idUser">ID user (freemium)</param>
        /// <param name="idClient">ID user MASTER</param>
        /// <param name="idProduct">ID product</param>
        /// <returns></returns>
        public bool SetProfileUser(int idProfile, int idUser, int idClient, int idProduct)
        {

            try
            {

                // User exists
                if(idUser != 0)
                {

                    // Find for subscription
                    var subscription = GetUserCurrentSubscription(idUser);

                    // Will check if profile is the same or not
                    if(subscription.IdProfile == idProfile)
                    {
                        this.lastError = "El perfil enviado es el mismo al actual del usuario";
                        return false;
                    }

                    // STEP A1: If it exists, will check for UserDimensions (trying to delete them)

                    var userDimensions = db.UsersDimensions.Where(e => e.IdSubscription == subscription.IdSubscription);
                    foreach(EntityFramework.UsersDimensions oUserDimension in userDimensions)
                    {
                        db.UsersDimensions.Remove(oUserDimension);
                    }

                    // STEP A2: Update user to new profile
                    subscription.IdProfile = idProfile;

                    // STEP A3: Recreate UserDimensions if new profile has consumible dimensions
                    // Get dimensions by profile
                    List<EntityFramework.Dimensions> dimensions = db.Dimensions.Where(e => e.ProfilesDimensions.Any(j => j.IdProfile == idProfile)).ToList();

                    if(dimensions.Count > 0)
                    {
                        foreach(var oDimension in dimensions)
                        {
                            // STEP A4: Check if dimension type is consumible
                            if(oDimension.IdDimensionType == (int)DimensionTypeEnum.CONSUMIBLE)
                            {
                                int idDimension = oDimension.IdDimension;

                                // Create new UserDimension
                                var userdimension = new EntityFramework.UsersDimensions();

                                userdimension.IdDimension = idDimension;
                                userdimension.DateCreated = DateTime.Now;
                                userdimension.DateLastUpdate = DateTime.Now;
                                userdimension.IdSubscription = subscription.IdSubscription;
                                userdimension.Active = true;

                                // Find value between profile and dimension
                                var profileDimension = db.ProfilesDimensions.Where(e => e.IdProfile == idProfile && e.IdDimension == idDimension).FirstOrDefault();
                                
                                // Set default value
                                userdimension.CurrentValue = profileDimension.Value;

                                db.UsersDimensions.Add(userdimension);

                            }
                        }
                    }

                    // Commit
                    db.SaveChanges();

                }
                else
                {

                    // If it doesn't exist, will set profile to user
                    using(EntityFramework.FriPriEntities context = new EntityFramework.FriPriEntities())
                    {

                        if(context.Database.Connection.State != ConnectionState.Open)
                        {
                            context.Database.Connection.Open();
                        }
                        
                        using(var transaction = context.Database.BeginTransaction())
                        {

                            try
                            {

                                // Create user
                                EntityFramework.Users newClient = new Repository.EntityFramework.Users
                                {
                                    Active = true,
                                    ExternalCode = idClient.ToString(), // ID user master
                                    IdProduct = idProduct
                                };

                                context.Users.Add(newClient);
                                context.SaveChanges();


                                // Create subscription
                                EntityFramework.Subscriptions newSubscription = new Repository.EntityFramework.Subscriptions
                                {
                                    Active = true,
                                    DateCreated = DateTime.Now,
                                    ExternalCode = "",
                                    IdProfile = idProfile,
                                    IdUser = newClient.IdUser,
                                    IsCurrent = true,
                                    RenewalDay = DateTime.Now.Day,
                                    LastRenewal = DateTime.Now
                                };

                                context.Subscriptions.Add(newSubscription);
                                context.SaveChanges();

                                // Get dimensions by profile
                                List<EntityFramework.Dimensions> dimensions = context.Dimensions.Where(e => e.ProfilesDimensions.Any(j => j.IdProfile == idProfile)).ToList();

                                if(dimensions.Count > 0)
                                {
                                    foreach(var oDimension in dimensions)
                                    {
                                        // STEP A4: Check if dimension type is consumible
                                        if(oDimension.IdDimensionType == (int)DimensionTypeEnum.CONSUMIBLE)
                                        {
                                            int idDimension = oDimension.IdDimension;

                                            // Create new UserDimension
                                            var userdimension = new EntityFramework.UsersDimensions();

                                            userdimension.IdDimension = idDimension;
                                            userdimension.DateCreated = DateTime.Now;
                                            userdimension.DateLastUpdate = DateTime.Now;
                                            userdimension.IdSubscription = newSubscription.IdSubscription;
                                            userdimension.Active = true;

                                            // Find value between profile and dimension
                                            var profileDimension = context.ProfilesDimensions.Where(e => e.IdProfile == idProfile && e.IdDimension == idDimension).FirstOrDefault();

                                            // Set default value
                                            userdimension.CurrentValue = profileDimension.Value;

                                            context.UsersDimensions.Add(userdimension);
                                            

                                        }
                                    }

                                    context.SaveChanges();
                                    transaction.Commit();
                                }

                            } catch(Exception e)
                            {
                                transaction.Rollback();
                                this.lastError = e.Message;

                                return false;
                            }
                            finally
                            {
                                if(context.Database.Connection.State == ConnectionState.Open)
                                {
                                    context.Database.Connection.Close();
                                }
                            }

           
                        }  
     
                    } // using


                } // end general if-else

                return true;

            }
            catch(Exception)
            {
                return false;
            }

        }



        public EntityFramework.Subscriptions GetUserCurrentSubscription(int idUser)
        {
            //primero obtengo la suscripcion del usuario por defecto
            var subscription = db.Subscriptions.FirstOrDefault(
                e=>
                e.IdUser == idUser && 
                e.IsCurrent == true && 
                e.Active == true
            );

            return subscription;
        }

        public EntityFramework.Subscriptions NewSubscription(EntityFramework.Subscriptions subs)
        {
            try
            {
                db.Subscriptions.Add(subs);
                db.SaveChanges();

                return subs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public EntityFramework.Subscriptions GetSubscription(int idSubscription)
        {
                return db.Subscriptions.FirstOrDefault(e => e.IdSubscription == idSubscription);
        }

        public EntityFramework.Subscriptions SetSubscriptionProfile(int idSubscription, int idProfile)
        {
            var sus = db.Subscriptions.FirstOrDefault(e => e.IdSubscription == idSubscription);

            if (sus == null)
                return null;

            sus.IdProfile = idProfile;
            sus.IdPromo = null;
            sus.PromoActive = null;
            sus.PromoEnd = null;
            sus.PromoFreeDays = null;
            sus.PromoStarted = null;

            db.SaveChanges();

            return sus;
        }

        public EntityFramework.Subscriptions SetSubscriptionProfileWithDays(int idSubscription, int idProfile, int days)
        {
            

            var sus = db.Subscriptions.FirstOrDefault(e => e.IdSubscription == idSubscription);

            if (sus == null)
                return null;

            sus.IdProfile = idProfile;

            if (sus.LastRenewal == null)
            {
                new EventLogRepository().SetLog("Agregando dias", "LastRenewal es nulo, se coloca la fecha de hoy ("+DateTime.Now+")");
                sus.LastRenewal = DateTime.Now;
            }

            
            new EventLogRepository().SetLog("Agregando dias", "LastRenewal tiene valor " + sus.LastRenewal.ToString());
            new EventLogRepository().SetLog("Agregando dias", "Se agregarán " + days + " dias");

            //agrega dias
            sus.LastRenewal = sus.LastRenewal.Value.AddDays(days);

            new EventLogRepository().SetLog("Agregando dias", "LastRenewal tiene valor " + sus.LastRenewal.ToString());

            db.SaveChanges();

            return sus;
        }

        public EntityFramework.Subscriptions SetSubscriptionProfileWithPromo(int idSubscription, int idProfile, EntityFramework.Promos promo)
        {
            new EventLogRepository().SetLog("SetSubscriptionProfileWithPromo", "Ingresa a metodo IDSUBS[" + idSubscription + "] IDPROF[" + idProfile + "]");

            var sus = db.Subscriptions.FirstOrDefault(e => e.IdSubscription == idSubscription);

            if (sus == null)
                return null;

            sus.IdProfile = idProfile;

            if (sus.LastRenewal == null)
            {
                new EventLogRepository().SetLog("Agregando dias", "LastRenewal es nulo, se coloca la fecha de hoy (" + DateTime.Now + ")");
                sus.LastRenewal = DateTime.Now;
            }


            new EventLogRepository().SetLog("Agregando dias", "LastRenewal tiene valor " + sus.LastRenewal.ToString());
            //new EventLogRepository().SetLog("Agregando dias", "Se agregarán " + days + " dias");

            //agrega dias
            //sus.LastRenewal = sus.LastRenewal.Value.AddDays(days);
            
            //se asigna la promo
            if (promo != null)
            {
                sus.IdProfile = promo.IdProfileActivePromo; //le setea el perfil que debe ir con la promo, independiente del que se envia por el servicio
                sus.PromoActive = true;
                sus.PromoStarted = DateTime.Now;
                sus.PromoEnd = DateTime.Now.AddDays((int)promo.FreeDays);
                sus.PromoFreeDays = promo.FreeDays;
                sus.IdPromo = promo.IdPromo;
            }

            new EventLogRepository().SetLog("Agregando dias", "LastRenewal tiene valor " + sus.LastRenewal.ToString());

            db.SaveChanges();

            return sus;
        }

        public List<EntityFramework.Subscriptions> GetSubscriptionsByRenewal(DateTime CurrentDate, int top)
        {
            //DateTime Today = DateTime.Now;
            int DaysInMonth = DateTime.DaysInMonth(CurrentDate.Year,CurrentDate.Month);

            //el primer dia del mes de la fecha actual
            DateTime fecha_limite_lastrenewal = CurrentDate.AddDays((CurrentDate.Day - 1) * (-1));

            //obtiene las subscripciones que no hayan sido renovadas este mes (Busca renovaciones de meses anteriores)
            var Subscriptions = db.Subscriptions.Where(e=> e.LastRenewal < fecha_limite_lastrenewal);

            //si el mes termina en este dia, que obtenga las suscripciones que sean de dia mayor (Todas las restantes del mes)
            if (CurrentDate.Day == DaysInMonth)
                Subscriptions = Subscriptions.Where(e => e.RenewalDay >= CurrentDate.Day);
            else
                Subscriptions = Subscriptions.Where(e => e.RenewalDay == CurrentDate.Day);

            //cantidad maxima
            if (top > 0)
                Subscriptions = Subscriptions.Take(top);

            return Subscriptions.ToList();

        }

        public List<ActiveSubscriptionsData> GetActiveSubscriptions(string ProductToken)
        {
            List<ActiveSubscriptionsData> list = new List<ActiveSubscriptionsData>();

            var subs = db.Subscriptions.Where(e=>e.Users.Products.Token == ProductToken && !e.Unsubscriptions.Any()).OrderBy(e=>e.DateCreated);

            foreach (var item in subs)
            {
                list.Add(new ActiveSubscriptionsData
                {
                    ProfileId = (int)item.IdProfile,
                    ProfileName = item.Profiles.Name,
                    ProfilePaid = (item.Profiles != null && item.Profiles.Paid != null) ? (bool)item.Profiles.Paid : false,
                    SubscriptionDateCreated = item.DateCreated,
                    UserExternalCode = item.Users.ExternalCode.Trim(),
                    SubscriptionDateLastRenewal = item.LastRenewal
                });
            }

            return list;
        }


    }
}
