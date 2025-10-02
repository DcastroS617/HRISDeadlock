using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDeprivationsBLL<T> where T : DeprivationEntity
    {
        /// <summary>
        /// List Individuals by Filters
        /// </summary>
        /// <param name="indicatorCode">Indicator Code</param>
        /// <param name="divisionCode">Division Name</param>
        /// <param name="companyCode">Company Name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <returns>List of Individual Entities</returns>
        PageHelper<DeprivationEntity> ListByFilters(
            int? indicatorCode,
            string divisionCode,
            string companyCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);

        /// <summary>
        /// List Surveys by Filters
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <returns>List of Survey Entities</returns>
        PageHelper<HouseholdDeprivationEntity> ListByEmployee(
            string employeeCode);
    }
}
