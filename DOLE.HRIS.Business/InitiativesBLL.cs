using System;
using System.Configuration;
using System.Linq;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using DOLE.HRIS.Entity;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business logic layer for Initiatives.
    /// </summary>
    public class InitiativesBll : IInitiativesBLL<InitiativeEntity>
    {
        private readonly IInitiativesDAL<InitiativeEntity> InitiativesDal;

        /// <summary>
        /// Constructor for InitiativesBll.
        /// </summary>
        /// <param name="objDal">Data access object for Initiatives.</param>
        public InitiativesBll(IInitiativesDAL<InitiativeEntity> objDal)
        {
            InitiativesDal = objDal;
        }

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="divisionCode">Division code.</param>
        /// <param name="companyCode">Company code.</param>
        /// <param name="costFarmId">Cost farm ID.</param>
        /// <param name="indicatorCode">Indicator code.</param>
        /// <param name="coordinatorCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<InitiativeEntity> ListByFilters(
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<InitiativeEntity> pageHelper = InitiativesDal.ListByFilters(
                     divisionCode,  companyCode, costFarmId,
                    indicatorCode, coordinatorCode,    
                     sortExpression, sortDirection, pageNumber.Value, null, pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
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
        /// List all Indicators
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>List of Indicator Entities</returns>
        public List<InitiativeEntity> ListAll(int divisionCode)
        {
            try
            {
                return InitiativesDal.ListAll(divisionCode);
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Division code.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public InitiativeEntity ListByKey(
            int? initiativeCode)
        {
            try
            {
                return InitiativesDal.ListByKey(initiativeCode); 
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Division code.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public Tuple<InitiativeEntity, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>> ListByKeyTuple(
            int? initiativeCode)
        {
            try
            {
                return InitiativesDal.ListByKeyTuple(initiativeCode);
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
        /// Save Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public DbaEntity InitiativesSave(InitiativeEntity entity, DataTable costZones, DataTable costMiniZones, DataTable costFarms, bool costZonesMarkAll, bool costMiniZonesMarkAll, bool costFarmsMarkAll)
        {
            try
            {
                return InitiativesDal.InitiativesSave(entity, costZones, costMiniZones, costFarms, costZonesMarkAll, costMiniZonesMarkAll, costFarmsMarkAll);
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
        /// Save Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public DbaEntity InitiativesDeactivate(InitiativeEntity entity)
        {
            try
            {
                return InitiativesDal.InitiativesDeactivate(entity);
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
    }
}
