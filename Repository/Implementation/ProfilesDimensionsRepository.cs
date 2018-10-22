using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ProfilesDimensionsRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        /// <summary>
        /// Obtiene dimensión del perfil actual para el usuario indicado
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="idUser"></param>
        /// <param name="DimensionTag"></param>
        /// <returns></returns>
        public ProfilesDimensions GetProfileDimension(int idUser, int idDimension)
        {
            return db.ProfilesDimensions.FirstOrDefault(e => e.Profiles.Subscriptions.Any(j => j.Users.IdUser == idUser && j.IsCurrent == true) && e.IdDimension == idDimension);
        }

        /// <summary>
        /// Get ProfileDimension in function of idProfile and idDimension
        /// </summary>
        /// <param name="idProfile">ID Profile</param>
        /// <param name="idDimension">ID dimension</param>
        /// <returns></returns>
        public ProfilesDimensions GetProfileDimensionPD(int idProfile, int idDimension)
        {
            return db.ProfilesDimensions.FirstOrDefault(e => e.IdProfile == idProfile && e.IdDimension == idDimension);
        }

        /// <summary>
        /// Get all profile dimensions related to profile
        /// </summary>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        public List<ProfilesDimensions> GetProfileDimensionsByIdProfile(int idProfile)
        {
            return db.ProfilesDimensions.Where(e => e.IdProfile == idProfile && e.Active == true).ToList();
        }

    }
}
