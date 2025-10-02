using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IInitiativeGoalsBLL
    {
        /// <summary>
        /// Lists initiative goals based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Initiative code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiative goals.</returns>
        PageHelper<InitiativeGoalEntity> ListByFilters(
            long initiativeCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);
    }
}
