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
    public class DimensionCategoriesService
    {
        private DimensionCategoriesRepository dimensionCategoriesRepository { get; set; }
        
        private Utilities utilities;

        public DimensionCategoriesService()
        {
            // Init repositories
            this.dimensionCategoriesRepository = new DimensionCategoriesRepository();
            this.utilities = new Utilities();
        }

        /// <summary>
        /// Get record By IdProduct an IdDimensionCategory
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ActionResponse GetDimensionCategoryByCriteriaAction(int idProduct, string criteria)
        {
            try
            {
                var dimensionCategories = dimensionCategoriesRepository.GetByCriteria(idProduct, criteria);

                if (dimensionCategories == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado registros", null);
                }

                DimensionCategory dc = new DimensionCategory
                {
                    idDimensionCategory = dimensionCategories.IdDimensionCategory,
                    description = dimensionCategories.Description,
                    idProduct = dimensionCategories.IdProduct,
                    active = dimensionCategories.Active,
                    tagName = dimensionCategories.TagName
                };

                return utilities.Response((int)CodeStatusEnum.OK, "OK", dc);
            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        /// <summary>
        /// Get all profiles related to product
        /// </summary>
        /// <returns></returns>
        public ActionResponse GetDimensionCategoriesAction(int idProduct)
        {
            try
            {
                // Find Records
                var dimensionCategories = dimensionCategoriesRepository.GetByProduct(idProduct);

                if (dimensionCategories.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado registros", null);
                }

                List<DimensionCategory> lst = new List<DimensionCategory>();

                foreach (var dimensionCategory in dimensionCategories)
                {
                    DimensionCategory dc = new DimensionCategory
                    {
                        idDimensionCategory = dimensionCategory.IdDimensionCategory,
                        description = dimensionCategory.Description,
                        idProduct = dimensionCategory.IdProduct,
                        active = dimensionCategory.Active,
                        tagName = dimensionCategory.TagName
                    };
                    
                    lst.Add(dc);
                }

                GetDimensionCategoryListResponse response = new GetDimensionCategoryListResponse();
                response.totalItems = lst.Count;
                response.items = lst;

                return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ActionResponse SetDimensionCategoryAction(int idProduct, dynamic dcData)
        {
            try
            {
                var checkDesc = dimensionCategoriesRepository.GetByCriteria(idProduct, dcData.description);
                var checkTagName = dimensionCategoriesRepository.GetByCriteria(idProduct, dcData.tagName);

                if (checkDesc != null || checkTagName != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }

                var action = dimensionCategoriesRepository.NewRecord(idProduct, dcData);

                if (action != null)
                {
                    DimensionCategory dc = new DimensionCategory
                    {
                        idDimensionCategory = action.IdDimensionCategory,
                        description = action.Description,
                        idProduct = action.IdProduct,
                        active = action.Active,
                        tagName = action.TagName
                    };

                    return utilities.Response((int)CodeStatusEnum.OK, "OK", dc);
                }
                else
                {
                    string msj = "No se pudo procesar la solicitud";
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, msj, null);
                }

            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ActionResponse UpdateDimensionCategoryAction(int idDimensionCategory, dynamic dcData)
        {
            try
            {
                var checkDesc = dimensionCategoriesRepository.GetByCriteria(dcData.idProduct, dcData.description);
                var checkTagName = dimensionCategoriesRepository.GetByCriteria(dcData.idProduct, dcData.tagName);

                if (checkDesc != null || checkTagName != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }

                var action = dimensionCategoriesRepository.UpdateRecord(idDimensionCategory, dcData);

                if (action != null)
                {
                    // Format response with RFC-3339
                    UpdateResponse response = new UpdateResponse
                    {
                        updated = DateTime.Now
                    };

                    return utilities.Response((int)CodeStatusEnum.OK, "OK", response);
                }
                else
                {
                    string msg = "No se pudo procesar la solicitud";
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, msg, null);
                }

            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }
    }
}
