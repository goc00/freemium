using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ReserveCodeRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();


        public Boolean AddReserveCode(ReserveCode obj)
        {
            //return db.Dimensions.FirstOrDefault(e => e.DimensionsCategories.IdProduct == idProduct && e.TagName == DimensionTag);
            return true;
        }

    }
}
