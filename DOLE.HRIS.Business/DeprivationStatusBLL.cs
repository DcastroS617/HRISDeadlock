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
    /// Business logic layer for Deprivation Status.
    /// </summary>
    public class DeprivationStatusBLL : IDeprivationStatusBLL<DeprivationStatusEntity>
    {
        private readonly IDeprivationStatusDAL<DeprivationStatusEntity> DeprivationStatusDal;

        /// <summary>
        /// Constructor for DeprivationStatusBLL.
        /// </summary>
        /// <param name="objDal">Data access object for Deprivation Status.</param>
        public DeprivationStatusBLL(IDeprivationStatusDAL<DeprivationStatusEntity> objDal)
        {
            DeprivationStatusDal = objDal;
        }

        /// <summary>
        /// Lists all deprivation statuses.
        /// </summary>
        /// <returns>List of all deprivation statuses.</returns>
        public List<DeprivationStatusEntity> ListAll()
        {
            try
            {
                return DeprivationStatusDal.ListAll();
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
        /// List the DeprivationStatus enabled
        /// </summary>
        /// <returns>The DeprivationStatus</returns>
        public List<DeprivationStatusEntity> ListEnabled()
        {
            try
            {
                return DeprivationStatusDal.ListEnabled();
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
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationStatus successfully added. False otherwise
        /// Second item: the DeprivationStatus added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, DeprivationStatusEntity> Add(DeprivationStatusEntity entity)
        {
            try
            {
                DeprivationStatusEntity previousEntity = DeprivationStatusDal.ListByNames(entity.DeprivationStatusDesSpanish, entity.DeprivationStatusDesEnglish);

                if (previousEntity == null)
                {
                    short DeprivationStatusCode = DeprivationStatusDal.Add(entity);
                    entity.DeprivationStatusCode = DeprivationStatusCode;

                    return new Tuple<bool, DeprivationStatusEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, DeprivationStatusEntity>(false, previousEntity);
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
        /// Edit the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>       
        public Tuple<bool, DeprivationStatusEntity> Edit(DeprivationStatusEntity entity)
        {
            try
            {
                DeprivationStatusEntity previousEntity = DeprivationStatusDal.ListByNames(entity.DeprivationStatusDesSpanish, entity.DeprivationStatusDesEnglish);

                if (previousEntity == null || previousEntity?.DeprivationStatusCode == entity.DeprivationStatusCode || previousEntity?.Deleted == false)
                {
                    DeprivationStatusDal.Edit(entity);

                    return new Tuple<bool, DeprivationStatusEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, DeprivationStatusEntity>(false, previousEntity);
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
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Delete(DeprivationStatusEntity entity)
        {
            try
            {
                DeprivationStatusDal.Delete(entity);
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
        /// Activate the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        public void Activate(DeprivationStatusEntity entity)
        {
            try
            {
                DeprivationStatusDal.Activate(entity);
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
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The Household Contribution Range </returns>
        public DeprivationStatusEntity ListByKey(short DeprivationStatusCode)
        {
            try
            {
                return DeprivationStatusDal.ListByKey(DeprivationStatusCode);
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
        /// List the DeprivationStatus by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationStatusNameSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusNameEnglish">The DeprivationStatus name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        public PageHelper<DeprivationStatusEntity> ListByFilters(int divisionCode, string DeprivationStatusNameSpanish, string DeprivationStatusNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<DeprivationStatusEntity> pageHelper = DeprivationStatusDal.ListByFilters(divisionCode
                    , DeprivationStatusNameSpanish
                    , DeprivationStatusNameEnglish
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
        /// List the DeprivationStatus By the spanish o english name
        /// </summary>
        /// <param name="DeprivationStatusNameSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusNameEnglish">The DeprivationStatus name english</param>
        /// <returns>The DeprivationStatus </returns>
        public DeprivationStatusEntity ListByNames(string DeprivationStatusNameSpanish, string DeprivationStatusNameEnglish)
        {
            try
            {
                return DeprivationStatusDal.ListByNames(DeprivationStatusNameSpanish, DeprivationStatusNameEnglish);
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

