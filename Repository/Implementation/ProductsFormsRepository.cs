using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ProductsFormsRepository
    {
        public EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public EntityFramework.ProductsForms GetProductForm(string token)
        {
            return db.ProductsForms.FirstOrDefault(e=>e.Token == token);
        }
    }
}
