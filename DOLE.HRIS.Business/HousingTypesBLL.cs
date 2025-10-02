using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class HousingTypesBll : IHousingTypesBll<HousingTypeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IHousingTypesDal<HousingTypeEntity> HousingTypesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public HousingTypesBll(IHousingTypesDal<HousingTypeEntity> objDal)
        {
            HousingTypesDal = objDal;
        }

        /// <summary>
        /// List the Housing Types enabled
        /// </summary>
        /// <returns>The Housing Types</returns>
        public List<HousingTypeEntity> ListEnabled()
        {
            try
            {
                return HousingTypesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        /// <returns>Tuple: En the first item a bool: true if Housing Type successfully added. False otherwise
        /// Second item: the Housing Type added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, HousingTypeEntity> Add(HousingTypeEntity entity)
        {
            try
            {
                HousingTypeEntity previousEntity = HousingTypesDal.ListByDescription(entity.HousingTypeDescriptionSpanish, entity.HousingTypeDescriptionEnglish);

                if (previousEntity == null)
                {
                    byte housingTypeCode = HousingTypesDal.Add(entity);
                    entity.HousingTypeCode = housingTypeCode;
                    return new Tuple<bool, HousingTypeEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, HousingTypeEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>       
        public Tuple<bool, HousingTypeEntity> Edit(HousingTypeEntity entity)
        {
            try
            {
                HousingTypeEntity previousEntity = HousingTypesDal.ListByDescription(entity.HousingTypeDescriptionSpanish, entity.HousingTypeDescriptionEnglish);

                if (previousEntity == null || previousEntity?.HousingTypeCode== entity.HousingTypeCode || previousEntity?.Deleted==false)
                {
                    HousingTypesDal.Edit(entity);

                    return new Tuple<bool, HousingTypeEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, HousingTypeEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        public void Delete(HousingTypeEntity entity)
        {
            try
            {
                HousingTypesDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        public void Activate(HousingTypeEntity entity)
        {
            try
            {
                HousingTypesDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Housing type By key
        /// </summary>
        /// <param name="housingTypeCode">The HousingType code</param>
        /// <returns>The Housing type</returns>
        public HousingTypeEntity ListByKey(byte housingTypeCode)
        {
            try
            {
                return HousingTypesDal.ListByKey(housingTypeCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Housing Type by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="housingTypeNameSpanish">The HousingType description spanish</param>
        /// <param name="housingTypeNameEnglish">The HousingType description english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The HousingType meeting the given filters and page config</returns>
        public PageHelper<HousingTypeEntity> ListByFilters(int divisionCode, string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<HousingTypeEntity> pageHelper = HousingTypesDal.ListByFilters(divisionCode
                    , housingTypeDescriptionSpanish
                    , housingTypeDescriptionEnglish
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , null
                    , pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesList, ex);
                }
            }
        }

        /// <summary>
        /// List the HousingType By the spanish o english name
        /// </summary>
        /// <param name="housingTypeNameSpanish">The HousingType name spanish</param>
        /// <param name="housingTypeNameEnglish">The HousingType name english</param>
        /// <returns>The HousingType </returns>
        public HousingTypeEntity ListByDescription(string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish)
        {
            try
            {
                return HousingTypesDal.ListByDescription(housingTypeDescriptionSpanish, housingTypeDescriptionEnglish);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgHousingTypesListByKey, ex);
                }
            }
        }
    }
}