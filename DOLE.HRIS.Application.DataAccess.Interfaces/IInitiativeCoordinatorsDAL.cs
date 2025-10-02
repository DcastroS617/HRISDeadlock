using System;
using System.Collections.Generic;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IInitiativeCoordinatorsDAL<T> where T : InitiativeCoordinatorEntity
    {
        /// <summary>
        /// List all Initiative Coordinators
        /// </summary>
        /// <returns>List of Coordinator Entities</returns>
        List<InitiativeCoordinatorEntity> ListAll(int divisionCode);

        // <summary>
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        int Add(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Delete(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// Activate the Coordinator
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Activate(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The DeprivationStatus</returns>
        InitiativeCoordinatorEntity ListByKey(int DeprivationStatusCode, int DivisionCode);

        /// <summary>
        /// List the DeprivationStatus by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        PageHelper<InitiativeCoordinatorEntity> ListByFilters(int? divisionCode, string CoordinatorName, string UserName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
    }
}
