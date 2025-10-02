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
    /// <summary>
    /// Business logic layer for Deprivation Institution.
    /// </summary>
    public class DeprivationInstitutionBLL : IDeprivationInstitutionBLL<DeprivationInstitutionEntity>
    {
        private readonly IDeprivationInstitutionDAL<DeprivationInstitutionEntity> DeprivationInstitutionDal;

        /// <summary>
        /// Constructor for DeprivationInstitutionBLL.
        /// </summary>
        /// <param name="objDal">Data access object for Deprivation Institution.</param>
        public DeprivationInstitutionBLL(IDeprivationInstitutionDAL<DeprivationInstitutionEntity> objDal)
        {
            DeprivationInstitutionDal = objDal;
        }

        /// <summary>
        /// Lists all deprivation institutions.
        /// </summary>
        /// <returns>List of all deprivation institutions.</returns>
        public List<DeprivationInstitutionEntity> ListAll()
        {
            try
            {
                return DeprivationInstitutionDal.ListAll();
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution enabled
        /// </summary>
        /// <returns>The DeprivationInstitution</returns>
        public List<DeprivationInstitutionEntity> ListEnabled()
        {
            try
            {
                return DeprivationInstitutionDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Add the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationInstitution successfully added. False otherwise
        /// Second item: the DeprivationInstitution added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, DeprivationInstitutionEntity> Add(DeprivationInstitutionEntity entity)
        {
            try
            {
                DeprivationInstitutionEntity previousEntity = DeprivationInstitutionDal.ListByNames(entity.DeprivationInstitutionDesSpanish, entity.DeprivationInstitutionDesEnglish);

                if (previousEntity == null)
                {
                    short DeprivationInstitutionCode = DeprivationInstitutionDal.Add(entity);
                    entity.DeprivationInstitutionCode = DeprivationInstitutionCode;

                    return new Tuple<bool, DeprivationInstitutionEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, DeprivationInstitutionEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Edit the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>       
        public Tuple<bool, DeprivationInstitutionEntity> Edit(DeprivationInstitutionEntity entity)
        {
            try
            {
                DeprivationInstitutionEntity previousEntity = DeprivationInstitutionDal.ListByNames(entity.DeprivationInstitutionDesSpanish, entity.DeprivationInstitutionDesEnglish);

                if (previousEntity == null || previousEntity?.DeprivationInstitutionCode == entity.DeprivationInstitutionCode || previousEntity?.Deleted == false)
                {
                    DeprivationInstitutionDal.Edit(entity);

                    return new Tuple<bool, DeprivationInstitutionEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, DeprivationInstitutionEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Delete the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public void Delete(DeprivationInstitutionEntity entity)
        {
            try
            {
                DeprivationInstitutionDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// Activate the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        public void Activate(DeprivationInstitutionEntity entity)
        {
            try
            {
                DeprivationInstitutionDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution By key
        /// </summary>
        /// <param name="DeprivationInstitutionCode">The DeprivationInstitution code</param>
        /// <returns>The Household Contribution Range </returns>
        public DeprivationInstitutionEntity ListByKey(short DeprivationInstitutionCode)
        {
            try
            {
                return DeprivationInstitutionDal.ListByKey(DeprivationInstitutionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationInstitutionNameSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionNameEnglish">The DeprivationInstitution name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The DeprivationInstitution meeting the given filters and page config</returns>
        public PageHelper<DeprivationInstitutionEntity> ListByFilters(int divisionCode, string DeprivationInstitutionNameSpanish, string DeprivationInstitutionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<DeprivationInstitutionEntity> pageHelper = DeprivationInstitutionDal.ListByFilters(divisionCode
                    , DeprivationInstitutionNameSpanish
                    , DeprivationInstitutionNameEnglish
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
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }

        /// <summary>
        /// List the DeprivationInstitution By the spanish o english name
        /// </summary>
        /// <param name="DeprivationInstitutionNameSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionNameEnglish">The DeprivationInstitution name english</param>
        /// <returns>The DeprivationInstitution </returns>
        public DeprivationInstitutionEntity ListByNames(string DeprivationInstitutionNameSpanish, string DeprivationInstitutionNameEnglish)
        {
            try
            {
                return DeprivationInstitutionDal.ListByNames(DeprivationInstitutionNameSpanish, DeprivationInstitutionNameEnglish);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParameters, ex);
                }
            }
        }
    }
}

