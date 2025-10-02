using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDeprivationStatusDAL<T> where T : DeprivationStatusEntity
    {
        /// <summary>
        /// List the DeprivationStatus enabled
        /// </summary>
        /// <returns>The Professions</returns>
        List<T> ListEnabled();

        List<T> ListAll();

        /// <summary>
        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        short Add(T entity);

        /// <summary>
        /// Edit the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        void Activate(T entity);

        /// <summary>
        /// List the DeprivationStatus By key
        /// </summary>
        /// <param name="DeprivationStatusCode">The DeprivationStatus code</param>
        /// <returns>The DeprivationStatus </returns>
        T ListByKey(short DeprivationStatusCode);

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
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the DeprivationStatus By the spanish o english name
        /// </summary>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <returns>The DeprivationStatus </returns>
        T ListByNames(string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish);
    }
}

