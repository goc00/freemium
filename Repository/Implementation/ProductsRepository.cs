using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ProductsRepository
    {
        private EntityFramework.FriPriEntities db = new EntityFramework.FriPriEntities();

        public int GetProductId(string ProductToken)
        {
            var product = db.Products.FirstOrDefault(e => e.Token == ProductToken);

            if (product == null)
                return 0;

            return product.IdProduct;
        }

        public EntityFramework.Products GetProduct(int IdProduct)
        {
            return db.Products.FirstOrDefault(e => e.IdProduct == IdProduct);
        }

        /// <summary>
        /// Return all records
        /// </summary>
        /// <returns></returns>
        public List<EntityFramework.Products> GetProducts()
        {
            var result = db.Products
                  .ToList();

            return result;
        }

    }
}