using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DimensionsRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        /// <summary>
        /// Get all product's dimensiones
        /// </summary>
        /// <param name="idProduct"></param>
        /// <returns></returns>
        public List<Dimensions> GetDimensionsByProduct(int idProduct)
        {
            var list = db.Dimensions.Where(e => e.DimensionsCategories.IdProduct == idProduct);

            return list.ToList();
        }

        /// <summary>
        /// Get dimension by type, category and tag name
        /// </summary>
        /// <param name="idDimensionType"></param>
        /// <param name="idDimensionCategory"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public Dimensions CheckValidDimension(int idDimensionType, int idDimensionCategory, string tagName)
        {
            return db.Dimensions.FirstOrDefault(
                entity => entity.IdDimensionType == idDimensionType 
                && entity.IdDimensionCategory == idDimensionCategory
                && entity.TagName == tagName
            );
        }

        public Dimensions GetDimension(int idProduct, string DimensionTag)
        {
            return db.Dimensions.FirstOrDefault(e => e.DimensionsCategories.IdProduct == idProduct && e.TagName == DimensionTag);
        }

        public Dimensions GetDimension(int idDimension)
        {
            return db.Dimensions.FirstOrDefault(e => e.IdDimension == idDimension);
        }

        public List<Dimensions> GetProfileDimensions(int IdProfile)
        {
            if (IdProfile == 0)
                return null;

            return db.Dimensions.Where(e=>e.ProfilesDimensions.Any(j=>j.IdProfile == IdProfile && j.Active == true)).ToList();
        }

        public List<Dimensions> GetProfileDimensionsByCategory(int IdProfile, string Category)
        {
            if (IdProfile == 0)
                return null;

            var dims = db.Dimensions.Where(e => e.ProfilesDimensions.Any(j => j.IdProfile == IdProfile));

            //filtra por categoria si es que la trae
            if (Category != null)
                dims = dims.Where(e=>e.DimensionsCategories.TagName == Category);

            return dims.ToList();
        }

        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Dimensions NewRecord(int idProduct, dynamic data)
        {
            try
            {
                var d = new Dimensions();

                d.Description = data.description;
                d.IdDimensionType = data.idDimensionType;
                d.IdDimensionCategory = data.idDimensionCategory;
                d.Value = data.value == -1 ? null : data.value;
                d.SwitchValue = data.switchValue == -1 ? null : Convert.ToBoolean(data.switchValue);
                d.Active = data.active == -1 ? true : Convert.ToBoolean(data.active);
                d.TagName = data.tagName;

                db.Dimensions.Add(d);
                db.SaveChanges();

                return d;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update Record
        /// </summary>
        /// <param name="idDimension"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Dimensions UpdateRecord(int idDimension, dynamic data)
        {
            try
            {
                var d = db.Dimensions.FirstOrDefault(
                    entity => entity.IdDimension == idDimension
                );

                if (d == null)
                {
                    return d;
                }

                if (!String.IsNullOrEmpty(data.description))
                {
                    d.Description = data.description;
                }

                if (data.idDimensionType != -1)
                {
                    d.IdDimensionType = data.idDimensionType;
                }

                if (data.idDimensionCategory != -1)
                {
                    d.IdDimensionCategory = data.idDimensionCategory;
                }

                if (data.value != -1)
                {
                    d.Value = data.value == null ? null : data.value;
                }

                if (data.switchValue != -1)
                {
                    d.SwitchValue = data.switchValue == null ? null : Convert.ToBoolean(data.switchValue);
                }

                if (data.active != -1)
                {
                    d.Active = Convert.ToBoolean(data.active);
                }

                if (!String.IsNullOrEmpty(data.tagName))
                {
                    d.TagName = data.tagName;
                }

                db.SaveChanges();

                return d;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
