using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PromosRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public EntityFramework.Promos GetPromo(int IdProduct, string TagPromo)
        {
            return db.Promos.FirstOrDefault(e=>e.IdProduct == IdProduct && e.TagPromo == TagPromo.Trim());
        }
    }
}
