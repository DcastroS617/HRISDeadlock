using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDeprivationManagementBLL<T> where T : DeprivationManagementEntity
    {
        PageHelper<T> ListByFilters(
            int? deprivationCode,
            string sortExpression,
            string sortDirection,
            int pageNumber);

        DbaEntity Save(T entity);

        DbaEntity Deactivate(T entity);

        /// <summary>
        /// Close deprivation management.
        /// </summary>
        DbaEntity CloseDeprivation(DeprivationManagementEntity entity);
    }
}
