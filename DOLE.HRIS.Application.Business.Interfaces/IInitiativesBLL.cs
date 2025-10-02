using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IInitiativesBLL<T> where T : InitiativeEntity
    {
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
        PageHelper<InitiativeEntity> ListByFilters(
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);

        /// <summary>
        /// List all Indicators
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>List of Indicator Entities</returns>
        List<InitiativeEntity> ListAll(int divisionCode);

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Division code.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        InitiativeEntity ListByKey(
            int? initiativeCode);

        // <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Division code.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        Tuple<InitiativeEntity, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>> ListByKeyTuple(
            int? initiativeCode);

        /// <summary>
        /// Save Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        DbaEntity InitiativesSave(InitiativeEntity entity, DataTable costZones, DataTable costMiniZones, DataTable costFarms, bool costZonesMarkAll, bool costMiniZonesMarkAll, bool costFarmsMarkAll);

        /// <summary>
        /// Deactivate Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        DbaEntity InitiativesDeactivate(InitiativeEntity entity);
    }


}
