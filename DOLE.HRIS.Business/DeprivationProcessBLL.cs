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
    /// Business logic layer for Deprivation Process.
    /// </summary>
    public class DeprivationProcessBLL : IDeprivationProcessBLL<DeprivationProcessEntity>
    {
        private readonly IDeprivationProcessDAL<DeprivationProcessEntity> DeprivationProcessDal;

        /// <summary>
        /// Constructor for DeprivationProcessBLL.
        /// </summary>
        /// <param name="objDal">Data access object for Deprivation Process.</param>
        public DeprivationProcessBLL(IDeprivationProcessDAL<DeprivationProcessEntity> objDal)
        {
            DeprivationProcessDal = objDal;
        }

        /// <summary>
        /// Lists all deprivation processes.
        /// </summary>
        /// <returns>List of all deprivation processes.</returns>
        public List<DeprivationProcessEntity> ListAll()
        {
            try
            {
                return DeprivationProcessDal.ListAll();
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
        /// List the DeprivationProcess enabled
        /// </summary>
        /// <returns>The DeprivationProcess</returns>
        public List<DeprivationProcessEntity> ListEnabled()
        {
            try
            {
                return DeprivationProcessDal.ListEnabled();
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
        /// Add the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationProcess successfully added. False otherwise
        /// Second item: the DeprivationProcess added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, DeprivationProcessEntity> Add(DeprivationProcessEntity entity)
        {
            try
            {
                DeprivationProcessEntity previousEntity = DeprivationProcessDal.ListByNames(entity.DeprivationProcessDesSpanish, entity.DeprivationProcessDesEnglish);

                if (previousEntity == null)
                {
                    short DeprivationProcessCode = DeprivationProcessDal.Add(entity);
                    entity.DeprivationProcessCode = DeprivationProcessCode;

                    return new Tuple<bool, DeprivationProcessEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, DeprivationProcessEntity>(false, previousEntity);
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
        /// Edit the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>       
        public Tuple<bool, DeprivationProcessEntity> Edit(DeprivationProcessEntity entity)
        {
            try
            {
                DeprivationProcessEntity previousEntity = DeprivationProcessDal.ListByNames(entity.DeprivationProcessDesSpanish, entity.DeprivationProcessDesEnglish);

                if (previousEntity == null || previousEntity?.DeprivationProcessCode == entity.DeprivationProcessCode || previousEntity?.Deleted == false)
                {
                    DeprivationProcessDal.Edit(entity);

                    return new Tuple<bool, DeprivationProcessEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, DeprivationProcessEntity>(false, previousEntity);
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
        /// Delete the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public void Delete(DeprivationProcessEntity entity)
        {
            try
            {
                DeprivationProcessDal.Delete(entity);
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
        /// Activate the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        public void Activate(DeprivationProcessEntity entity)
        {
            try
            {
                DeprivationProcessDal.Activate(entity);
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
        /// List the DeprivationProcess By key
        /// </summary>
        /// <param name="DeprivationProcessCode">The DeprivationProcess code</param>
        /// <returns>The Household Contribution Range </returns>
        public DeprivationProcessEntity ListByKey(short DeprivationProcessCode)
        {
            try
            {
                return DeprivationProcessDal.ListByKey(DeprivationProcessCode);
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
        /// List the DeprivationProcess by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationProcessNameSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessNameEnglish">The DeprivationProcess name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The DeprivationProcess meeting the given filters and page config</returns>
        public PageHelper<DeprivationProcessEntity> ListByFilters(int divisionCode, string DeprivationProcessNameSpanish, string DeprivationProcessNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<DeprivationProcessEntity> pageHelper = DeprivationProcessDal.ListByFilters(divisionCode
                    , DeprivationProcessNameSpanish
                    , DeprivationProcessNameEnglish
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
        /// List the DeprivationProcess By the spanish o english name
        /// </summary>
        /// <param name="DeprivationProcessNameSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessNameEnglish">The DeprivationProcess name english</param>
        /// <returns>The DeprivationProcess </returns>
        public DeprivationProcessEntity ListByNames(string DeprivationProcessNameSpanish, string DeprivationProcessNameEnglish)
        {
            try
            {
                return DeprivationProcessDal.ListByNames(DeprivationProcessNameSpanish, DeprivationProcessNameEnglish);
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

