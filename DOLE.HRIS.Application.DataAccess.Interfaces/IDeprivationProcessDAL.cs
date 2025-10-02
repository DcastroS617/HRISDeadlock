using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDeprivationProcessDAL<T> where T : DeprivationProcessEntity
    {
        /// <summary>
        /// List the DeprivationProcess enabled
        /// </summary>
        /// <returns>The Professions</returns>
        List<T> ListEnabled();

        List<T> ListAll();

        /// <summary>
        /// Add the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        short Add(T entity);

        /// <summary>
        /// Edit the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        void Activate(T entity);

        /// <summary>
        /// List the DeprivationProcess By key
        /// </summary>
        /// <param name="DeprivationProcessCode">The DeprivationProcess code</param>
        /// <returns>The DeprivationProcess </returns>
        T ListByKey(short DeprivationProcessCode);

        /// <summary>
        /// List the DeprivationProcess by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationProcessDesSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessDesEnglish">The DeprivationProcess name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationProcess meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the DeprivationProcess By the spanish o english name
        /// </summary>
        /// <param name="DeprivationProcessDesSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessDesEnglish">The DeprivationProcess name english</param>
        /// <returns>The DeprivationProcess </returns>
        T ListByNames(string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish);
    }
}

