using Business.Libraries;
using Contract.Models.Dto;
using Contract.Models.Enum;
using Contract.Models.Response;
using Contract.Models.Response.Common;
using Repository.Implementation;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Business.Implementation
{
    public class DimensionTypesService
    {
        private DimensionTypesRepository dimensionTypesRepository { get; set; }
        
        private Utilities utilities;

        public DimensionTypesService()
        {
            // Init repositories
            this.dimensionTypesRepository = new DimensionTypesRepository();
            this.utilities = new Utilities();
        }


        /// <summary>
        /// Get all profiles related to product
        /// </summary>
        /// <returns></returns>
        public ActionResponse GetDimensionTypesAction()
        {
            try
            {
                // Find Dimension Types
                var dimensionTypes = dimensionTypesRepository.GetDimensionTypes();

                if (dimensionTypes.Count <= 0)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado perfiles asociados al producto", null);
                }

                List<DimensionType> lst = new List<DimensionType>();

                foreach (var dimensionType in dimensionTypes)
                {
                    DimensionType dt = new DimensionType
                    {
                        idDimensionType = dimensionType.IdDimensionType,
                        description = dimensionType.Description,
                        active = dimensionType.Active,
                        tagName = dimensionType.TagName
                    };

                    lst.Add(dt);
                }

                GetDimensionTypeListResponse response = new GetDimensionTypeListResponse
                {
                    totalItems = lst.Count,
                    items = lst
                };

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
        public ActionResponse SetDimensionTypesAction(string description, string tagName)
        {
            try
            {
                // Check if dimension type exists
                var checkDimension = this.dimensionTypesRepository.GetDimensionType(description, tagName);

                if (checkDimension != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }

                // Set or update all related to profile
                var action = dimensionTypesRepository.NewRecord(description, tagName);

                if (action != null)
                {
                    return utilities.Response((int)CodeStatusEnum.OK, "OK", action);
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
    }
}
