using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;


namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IInitiativeCoordinatorsBLL<T> where T : InitiativeCoordinatorEntity
    {
        /// <summary>
        /// Lists all initiative coordinators.
        /// </summary>
        /// <returns>List of all initiative coordinators.</returns>
        List<InitiativeCoordinatorEntity> ListAll(int divisionCode);

        /// <summary>
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationStatus successfully added. False otherwise
        /// Second item: the DeprivationStatus added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, InitiativeCoordinatorEntity> Add(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Delete(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// Activate the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Activate(InitiativeCoordinatorEntity entity);

        /// <summary>
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The Household Contribution Range </returns>
        InitiativeCoordinatorEntity ListByKey(int DeprivationStatusCode, int DivisionCode);

        /// <summary>
        /// List the DeprivationStatus by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="CoordinatorName">The DeprivationStatus name spanish</param>
        /// <param name="UserName">The DeprivationStatus name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        PageHelper<InitiativeCoordinatorEntity> ListByFilters(int? divisionCode, string CoordinatorName, string UserName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
    }
}
