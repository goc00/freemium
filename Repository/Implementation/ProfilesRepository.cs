using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ProfilesRepository
    {
        public EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public EntityFramework.Profiles GetProfile(int IdProfile)
        {
            return db.Profiles.FirstOrDefault(e => e.IdProfile == IdProfile);
        }

        public EntityFramework.Profiles GetStandardFreeSuscription(int idUser)
        {
            //obtengo al usuario
            var user = db.Users.FirstOrDefault(e => e.IdUser == idUser);

            //obtengo al perfil estándar free del producto al que pertenece el usuario
            return db.Profiles.FirstOrDefault(e => e.IdProduct == user.IdProduct && e.UserDefault == true);
        }

        public List<EntityFramework.Profiles> GetProfiles(int idProduct)
        {
            return db.Profiles.Where(e=>e.IdProduct == idProduct).ToList();
        }

        public List<EntityFramework.Profiles> GetPaidProfiles(int idProduct)
        {
            return db.Profiles.Where(e => e.IdProduct == idProduct && e.Paid == true).ToList();
        }

        public Profiles GetProfileByAnon(int IdProduct)
        {
            //return db.ProfilesDimensions.FirstOrDefault(e=>e.Profiles.IdProduct == IdProduct && e.Profiles.AnonDefault == true);
            return db.Profiles.FirstOrDefault(e => e.IdProduct == IdProduct && e.AnonDefault == true);
        }

        /// <summary>
        /// Get Profile by Name or TagName
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Profiles GetProfileByNameOrTagName(int idProduct, string criteria)
        {
            return db.Profiles.FirstOrDefault(
                e => (e.IdProduct == idProduct && e.Name == criteria)
                || (e.IdProduct == idProduct && e.TagName == criteria)
            );
        }


        /// <summary>
        /// Get Profiles by country
        /// </summary>
        /// <param name="idProduct">ID product</param>
        /// <param name="alpha2">Alpha-2 (2 letters) country</param>
        /// <returns></returns>
        public List<EntityFramework.Profiles> GetProfilesByCountry(int idProduct, string alpha2)
        {
            return db.Profiles.Where(e => e.IdProduct == idProduct && e.Country == alpha2).ToList();
        }


        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="bodyObj"></param>
        /// <returns></returns>
        public Profiles NewRecord(int idProduct, dynamic bodyObj)
        {
            try
            {
                var p = new Profiles();
                
                p.IdProduct = idProduct;
                p.Name = bodyObj.Name;
                p.Description = bodyObj.Description;
                p.PriceUSD = null;
                p.Active = true;
                p.TagName = bodyObj.TagName;
                p.AnonDefault = bodyObj.AnonDefault;
                p.UserDefault = bodyObj.UserDefault;
                p.Paid = bodyObj.Paid;
                p.MotivatorText = null;
                p.ShortDescription = null;

                db.Profiles.Add(p);
                db.SaveChanges();

                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update record dynamically
        /// </summary>
        /// <param name="idProfile"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Profiles UpdateRecord(int idProfile, dynamic data)
        {
            try
            {
                int idProduct = data.idProduct; // Can't use data.IdProduct directly

                var p = db.Profiles.FirstOrDefault(
                    e => e.IdProfile == idProfile && e.IdProduct == idProduct
                );

                if (p == null) // Check if element exists
                {
                    return p;
                }

                // Evaluate permitted records 

                if (data.name != null)
                {
                    p.Name = data.name;
                }

                if (data.description != null)
                {
                    p.Description = data.description;
                }

                if (data.priceUSD != null)
                {
                    p.PriceUSD = data.priceUSD;
                }

                if (data.active == 0 || data.active == 1)
                {
                    p.Active = Convert.ToBoolean(data.active);
                }

                if (data.tagName != null)
                {
                    p.TagName = data.tagName;
                }

                if (data.anonDefault == 0 || data.anonDefault == 1)
                {
                    p.AnonDefault = Convert.ToBoolean(data.anonDefault);
                }

                if (data.userDefault == 0 || data.userDefault == 1)
                {
                    p.UserDefault = Convert.ToBoolean(data.userDefault);
                }

                if (data.paid == 0 || data.paid == 1)
                {
                    p.Paid = Convert.ToBoolean(data.paid);
                }

                if (data.featured == 0 || data.featured == 1)
                {
                    p.Featured = Convert.ToBoolean(data.featured);
                }

                db.SaveChanges(); // Update record

                return p; // Return record
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
