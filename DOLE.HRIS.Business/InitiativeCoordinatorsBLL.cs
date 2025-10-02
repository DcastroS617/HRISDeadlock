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
    /// Business logic layer for Initiative Coordinators.
    /// </summary>
    public class InitiativeCoordinatorsBll : IInitiativeCoordinatorsBLL<InitiativeCoordinatorEntity>
    {
        private readonly IInitiativeCoordinatorsDAL<InitiativeCoordinatorEntity> InitiativeCoordinatorsDal;

        /// <summary>
        /// Constructor for InitiativeCoordinatorsBll.
        /// </summary>
        /// <param name="objDal">Data access object for Initiative Coordinators.</param>
        public InitiativeCoordinatorsBll(IInitiativeCoordinatorsDAL<InitiativeCoordinatorEntity> objDal)
        {
            InitiativeCoordinatorsDal = objDal;
        }

        /// <summary>
        /// Lists all initiative coordinators.
        /// </summary>
        /// <returns>List of all initiative coordinators.</returns>
        public List<InitiativeCoordinatorEntity> ListAll(int divisionCode)
        {
            try
            {
                return InitiativeCoordinatorsDal.ListAll(divisionCode);
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
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationStatus successfully added. False otherwise
        /// Second item: the DeprivationStatus added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, InitiativeCoordinatorEntity> Add(InitiativeCoordinatorEntity entity)
        {
            try
            {
                int CoordinatorCode = InitiativeCoordinatorsDal.Add(entity);
                InitiativeCoordinatorEntity previousEntity = InitiativeCoordinatorsDal.ListByKey(CoordinatorCode, entity.DivisionCode);

                if (previousEntity == null)
                {
                    entity.CoordinatorCode = CoordinatorCode;

                    return new Tuple<bool, InitiativeCoordinatorEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, InitiativeCoordinatorEntity>(false, previousEntity);
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
        public void Delete(InitiativeCoordinatorEntity entity)
        {
            try
            {
                InitiativeCoordinatorsDal.Delete(entity);
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
        public void Activate(InitiativeCoordinatorEntity entity)
        {
            try
            {
                InitiativeCoordinatorsDal.Activate(entity);
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
        public InitiativeCoordinatorEntity ListByKey(int DeprivationStatusCode, int DivisionCode)
        {
            try
            {
                return InitiativeCoordinatorsDal.ListByKey(DeprivationStatusCode, DivisionCode);
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
        public PageHelper<InitiativeCoordinatorEntity> ListByFilters(int? divisionCode, string CoordinatorName, string UserName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<InitiativeCoordinatorEntity> pageHelper = InitiativeCoordinatorsDal.ListByFilters(divisionCode
                    , CoordinatorName
                    , UserName
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
    }
}

