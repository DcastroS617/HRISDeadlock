using System;
using System.Collections.Generic;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IInitiativesDAL<T> where T :InitiativeEntity
    {
        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <param name="costFarmId">Cost Farm ID</param>
        /// <param name="indicatorCode">Indicator Code</param>
        /// <param name="coordinatorCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<InitiativeEntity> ListByFilters(
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// List all Indicators
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>List of Indicator Entities</returns>
        List<InitiativeEntity> ListAll(int divisionCode);

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Division Code</param>
        /// <returns>List of Initiative Entities</returns>
        InitiativeEntity ListByKey(
            int? initiativeCode);

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Division Code</param>
        Tuple<InitiativeEntity, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>> ListByKeyTuple(
            int? initiativeCode);

        /// <summary>
        /// Save Initiative 
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        DbaEntity InitiativesSave(InitiativeEntity entity, DataTable costZones, DataTable costMiniZones, DataTable costFarms, bool costZonesMarkAll, bool costMiniZonesMarkAll, bool costFarmsMarkAll);

        /// <summary>
        /// Deactivate the Initiative
        /// </summary>
        /// <param name="entity">Initiative</param>
        DbaEntity InitiativesDeactivate(InitiativeEntity entity);

    }


}
