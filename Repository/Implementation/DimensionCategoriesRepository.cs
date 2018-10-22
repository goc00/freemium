using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Implementation
{
    public class DimensionCategoriesRepository
    {
        private FriPriEntities db = new FriPriEntities();

        public List<DimensionsCategories> GetByProduct(int idProduct)
        {
            return db.DimensionsCategories.Where(
                e => e.IdProduct == idProduct
            ).ToList();
        }

        public DimensionsCategories GetByCriteria(int idProduct, string criteria)
        {
            return db.DimensionsCategories.FirstOrDefault(
                e => (e.IdProduct == idProduct && e.Description == criteria)
                || (e.IdProduct == idProduct && e.TagName == criteria)
            );
        }

        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public DimensionsCategories NewRecord(int idProduct, dynamic data)
        {
            try
            {
                var d = new DimensionsCategories
                {
                    IdProduct = idProduct,
                    Description = data.description,
                    TagName = data.tagName,
                    Active = data.active == -1 ? true : Convert.ToBoolean(data.active)
                };

                db.DimensionsCategories.Add(d);
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
        public DimensionsCategories UpdateRecord(int idDimensionCategory, dynamic data)
        {
            try
            {
                var d = db.DimensionsCategories.FirstOrDefault(
                    entity => entity.IdDimensionCategory == idDimensionCategory
                );

                if (d == null)
                {
                    return d;
                }

                if (!String.IsNullOrEmpty(data.description))
                {
                    d.Description = data.description;
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
