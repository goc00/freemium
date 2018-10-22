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
    public class ProfileDimensionsService
    {
        private ProfileDimensionsRepository profileDimensionsRepository { get; set; }
        
        private Utilities utilities;

        public ProfileDimensionsService()
        {
            // Init repositories
            profileDimensionsRepository = new ProfileDimensionsRepository();
            utilities = new Utilities();
        }

        /// <summary>
        /// Get record By IdProduct an IdDimensionCategory
        /// </summary>
        /// <param name="idProduct"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public ActionResponse GetProfileDimensionsByProductAction(int idProduct)
        {
            try
            {
                var profileDimensions = profileDimensionsRepository.GetProfileDimensions(idProduct);

                if (profileDimensions == null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "No se han encontrado registros", null);
                }

                List<ProfileDimension> lst = new List<ProfileDimension>();

                foreach (var dimensionProfile in profileDimensions)
                {
                    ProfileDimension pd = new ProfileDimension
                    {
                        idProfileDimension = dimensionProfile.IdProfileDimension,
                        idProfile = dimensionProfile.IdProfile,
                        idDimension = dimensionProfile.IdDimension,
                        value = dimensionProfile.Value,
                        switchValue = dimensionProfile.SwitchValue,
                        active = dimensionProfile.Active,
                        idProduct = dimensionProfile.Profiles.IdProduct
                    };

                    lst.Add(pd);
                }

                GetProfileDimensionListResponse response = new GetProfileDimensionListResponse
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
        public ActionResponse SetProfileDimensionAction(int idProduct, dynamic pdData)
        {
            try
            {
                var checkPd = profileDimensionsRepository.GetByProfileAndDimension(pdData.idProfile, pdData.idDimension);

                if (checkPd != null)
                {
                    return utilities.Response((int)CodeStatusEnum.NO_CONTENT, "La entidad ya existe", null);
                }

                var action = profileDimensionsRepository.NewRecord(idProduct, pdData);

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

        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="description"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public ActionResponse UpdateProfileDimensionAction(int idProfileDimension, dynamic PdData)
        {
            try
            {
                var action = profileDimensionsRepository.UpdateRecord(idProfileDimension, PdData);

                if (action != null)
                {
                    return utilities.Response((int)CodeStatusEnum.OK, "OK", action);
                }
                else
                {
                    //string msg = "No se pudo procesar la solicitud";
                    return utilities.Response((int)CodeStatusEnum.CONFLICT, profileDimensionsRepository.GetLastError(), null);
                }

            }
            catch (Exception e)
            {
                return utilities.Response((int)CodeStatusEnum.INTERNAL_ERROR, "Error desconocido en el sistema: " + e.Message, null);
            }
        }
    }
}
