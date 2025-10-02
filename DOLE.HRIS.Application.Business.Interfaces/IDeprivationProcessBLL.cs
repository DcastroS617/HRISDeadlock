using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDeprivationProcessBLL<T> where T : DeprivationProcessEntity
    {
        List<T> ListAll();

        /// <summary>
        /// List the DeprivationProcess enabled
        /// </summary>
        /// <returns>The DeprivationProcess</returns>
        List<T> ListEnabled();

        /// Add the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationProcess successfully added. False otherwise
        /// Second item: the DeprivationProcess added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the DeprivationProcess
        /// </summary>
        /// <param name="entity">The DeprivationProcess</param>        
        Tuple<bool, DeprivationProcessEntity> Edit(T entity);

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
        /// <param name="DeprivationProcessCode">The DeprivationProcess</param>
        /// <returns>The DeprivationProcess</returns>
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
        /// <returns>The DeprivationProcess meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the DeprivationProcess By the spanish o english name
        /// </summary>
        /// <param name="DeprivationProcessDesSpanish">The DeprivationProcess name spanish</param>
        /// <param name="DeprivationProcessDesEnglish">The DeprivationProcess name english</param>
        /// <returns>The DeprivationProcess </returns>
        T ListByNames(string DeprivationProcessDesSpanish, string DeprivationProcessDesEnglish);
    }
}

