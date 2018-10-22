using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Implementation
{
    public class ProfileDimensionsRepository
    {
        private FriPriEntities db = new FriPriEntities();
        private string lastError = "";

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public List<ProfilesDimensions> GetProfileDimensions(int IdProduct)
        {
            return db.ProfilesDimensions.Where(entity => entity.Profiles.IdProduct == IdProduct).ToList();
        }

        /// <summary>
        /// Get record by profile and dimension
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ProfilesDimensions GetByProfileAndDimension(int idProfile, int idDimension)
        {
            return db.ProfilesDimensions.FirstOrDefault(
                entity => entity.IdProfile == idProfile && entity.IdDimension == idDimension
            );
        }

        /// <summary>
        /// Get by profile
        /// </summary>
        /// <param name="idProfile"></param>
        /// <returns></returns>
        public ProfilesDimensions GetByProfile(int idProfile)
        {
            return db.ProfilesDimensions.FirstOrDefault(
                entity => entity.IdProfile == idProfile
            );
        }

        /// <summary>
        /// Get by dimension
        /// </summary>
        /// <param name="idDimension"></param>
        /// <returns></returns>
        public ProfilesDimensions GetByDimension(int idDimension)
        {
            return db.ProfilesDimensions.FirstOrDefault(
                entity => entity.IdDimension == idDimension
            );
        }

        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ProfilesDimensions NewRecord(int idProduct, dynamic data)
        {
            try
            {
                var pd = new ProfilesDimensions
                {
                    IdProfile = data.idProfile,
                    IdDimension = data.idDimension,
                    Value = data.value == -1 ? null : data.value,
                    SwitchValue = data.switchValue == -1 ? null : Convert.ToBoolean(data.switchValue),
                    Active = data.active == -1 ? true : Convert.ToBoolean(data.active)
                };

                db.ProfilesDimensions.Add(pd);
                db.SaveChanges();

                return pd;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update row
        /// </summary>
        /// <param name="idDimensionCategory"></param>
        /// <param name="description"></param>
        /// <param name="idProduct"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ProfilesDimensions UpdateRecord(int idProfileDimension, dynamic data)
        {
            try
            {

                DimensionsRepository dr = new DimensionsRepository();
                ProfilesRepository pr = new ProfilesRepository();

                // Get profile and dimension object to validate data integrity
                var oDimension = dr.GetDimension(data.idDimension);
                var oProfile = pr.GetProfile(data.idProfile);

                if(oDimension == null)
                {
                    throw new Exception("No se ha podido determinar la dimensión seleccionada");
                }

                if(oProfile == null)
                {
                    throw new Exception("No se ha podido determinar el perfil seleccionado");
                }

                // Check for profile - product
                if(oProfile.IdProduct != data.idProduct)
                {
                    throw new Exception("El perfil seleccionado con coincide con el producto enviado");
                }


                // Get ProfileDimension object
                var pd = db.ProfilesDimensions.FirstOrDefault(
                    entity => entity.IdProfileDimension == idProfileDimension
                );

                if (pd == null)
                {
                    return pd;
                }

                if (data.idProfile != -1)
                {
                    pd.IdProfile = data.idProfile;
                }

                if (data.idDimension != -1)
                {
                    pd.IdDimension = data.idDimension;
                }

                // Check for value or switchValue in function of type
                if(oDimension.IdDimensionType == 2)
                {
                    if(data.switchValue != -1)
                    {
                        pd.SwitchValue = Convert.ToBoolean(data.switchValue);
                    }
                    else
                    {
                        throw new Exception("No se ha encontrado el valor a actualizar (boolean)");
                    }
                }
                else
                {
                    if(data.value != -1)
                    {
                        pd.Value = data.value;
                    }
                    else
                    {
                        throw new Exception("No se ha encontrado el valor a actualizar (numeric)");
                    }
                }

 
                if (data.active != -1)
                {
                    pd.Active = Convert.ToBoolean(data.active);
                }

                db.SaveChanges();

                return pd;
            }
            catch (Exception e)
            {
                this.lastError = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Get last message error from actions in repository
        /// </summary>
        /// <returns></returns>
        public string GetLastError()
        {
            return this.lastError;
        }
       
    }
}
