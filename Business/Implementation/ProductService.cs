using Business.Libraries;
using Contract.Models.Dto;
using Contract.Models.Enum;
using Contract.Models.Response;
using Contract.Models.Response.Common;
using Repository.Implementation;
using System;
using System.Collections.Generic;

namespace Business.Implementation
{
    public class ProductService
    {
        private ProductsRepository productsRepository { get; set; }
        
        private Utilities utilities;

        public ProductService()
        {
            // Init repositories
            this.productsRepository = new ProductsRepository();
            this.utilities = new Utilities();
        }

        /// <summary>
        /// Get all profiles related to product
        /// </summary>
        /// <returns></returns>
        public ActionResponse GetProductsAction()
        {
            try
            {
                // Find Records
                var products = productsRepository.GetProducts();

                if (products.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado registros", null);
                }

                List<Product> lst = new List<Product>();

                foreach (var product in products)
                {
                    Product p = new Product
                    {
                        idProduct = product.IdProduct,
                        description = product.Description,
                        token = product.Token,
                        tagName = product.TagName
                    };

                    lst.Add(p);
                }

                GetProductListResponse response = new GetProductListResponse();
                response.totalItems = lst.Count;
                response.items = lst;

                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }
    }
}
