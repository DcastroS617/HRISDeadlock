using System;
using System.Collections.Generic;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;


namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    /// <summary>
    /// List Initiative Goals by Filters
    /// </summary>
    /// <param name="initiativeCode">Initiative Code</param>
    /// <param name="sortExpression">Sort Expression</param>
    /// <param name="sortDirection">Sort Direction</param>
    /// <param name="pageNumber">Page Number</param>
    /// <param name="pageSize">Page Size</param>
    /// <param name="pageSizeValue">Page Size Value</param>
    /// <returns>List of Initiative Goal Entities</returns>
    public interface IInitiativeGoalsDAL
    {
        PageHelper<InitiativeGoalEntity> ListByFilters(
            long initiativeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);
    }
}
