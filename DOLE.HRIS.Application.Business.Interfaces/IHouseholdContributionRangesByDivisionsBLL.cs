using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IHouseholdContributionRangesByDivisionsBll<T> where T : HouseHoldContributionRangeByDivisionEntity
    {
        /// <summary>
        /// List the household contribution ranges by division enabled
        /// </summary>
        /// <returns>The household contribution ranges by division</returns>
        List<T> ListEnabled(bool? SearchEnabled = null);

        /// Add the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        /// <returns>Tuple: En the first item a bool: true if Household Contribution Range By Division successfully added. False otherwise
        /// Second item: the Household Contribution Range By Division added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>        
        void Edit(T entity);

        /// <summary>
        /// Delete the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the Household Contribution Range By Division
        /// </summary>
        /// <param name="entity">The Household Contribution Range By Division</param>
        int Activate(T entity);

        /// <summary>
        /// List the Household Contribution Range By Division and by order
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="rangeOrder">The range order</param>
        /// <returns>The Household Contribution Range By Division and by range order</returns>
        T ListByDivisionByOrder(int divisionCode, byte rangeOrder);

        /// <summary>
        /// List the Household Contribution Range By key
        /// </summary>
        /// <param name="householdContributionRangeCode">The household contribution range code</param>
        /// <returns>The Household Contribution Range </returns>
        T ListByKey(short householdContributionRangeCode);

        /// <summary>
        /// List the Household Contribution Range by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Household Contribution Range meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
    }
}