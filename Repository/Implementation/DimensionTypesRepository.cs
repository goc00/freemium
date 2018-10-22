using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class DimensionTypesRepository
    {
        private FriPriEntities db = new FriPriEntities();

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public List<DimensionsTypes> GetDimensionTypes()
        {
            return db.DimensionsTypes.ToList();
        }

        /// <summary>
        /// Get record by description or tagName
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public DimensionsTypes GetDimensionType(string description, string tagName)
        {
            return db.DimensionsTypes.FirstOrDefault(entity => entity.Description == description || entity.TagName == tagName);
        }

        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public DimensionsTypes NewRecord(string description, string tagName)
        {
            try
            {
                var dt = new DimensionsTypes();

                dt.Description = description;
                dt.Active = true;
                dt.TagName = tagName;

                db.DimensionsTypes.Add(dt);
                db.SaveChanges();

                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
