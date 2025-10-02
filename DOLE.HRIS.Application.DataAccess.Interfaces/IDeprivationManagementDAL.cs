using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDeprivationManagementDAL<T> where T : DeprivationManagementEntity
    {
        /// <summary>
        /// List Deprivation Management entries by filters.
        /// </summary>
        /// <param name="deprivationCode">Deprivation Institution Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>Paged list of DeprivationManagement entities</returns>
        PageHelper<DeprivationManagementEntity> ListByFilters(
            int? deprivationCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// Get Deprivation Management by its unique key.
        /// </summary>
        /// <param name="deprivationCode">Deprivation Code</param>
        /// <returns>A single DeprivationManagement entity</returns>
        DeprivationManagementEntity ListByKey(int deprivationCode);

        
        /// <summary>
        /// Save a Deprivation Management entry.
        /// </summary>
        /// <param name="entity">DeprivationManagement entity</param>
        /// <returns>DbaEntity with the result of the operation</returns>
        DbaEntity Save(DeprivationManagementEntity entity);

        /// <summary>
        /// Deactivate a Deprivation Management entry.
        /// </summary>
        /// <param name="entity">DeprivationManagement entity</param>
        /// <returns>DbaEntity with the result of the operation</returns>
        DbaEntity Deactivate(DeprivationManagementEntity entity);

        /// <summary>
        /// Close deprivation Management.
        /// </summary>
        DbaEntity CloseDeprivation(DeprivationManagementEntity entity);
    }
}
