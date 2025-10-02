using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDeprivationStatusBLL<T> where T : DeprivationStatusEntity
    {
        List<T> ListAll();

        /// <summary>
        /// List the DeprivationStatus enabled
        /// </summary>
        /// <returns>The DeprivationStatus</returns>
        List<T> ListEnabled();

        /// Add the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationStatus successfully added. False otherwise
        /// Second item: the DeprivationStatus added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the DeprivationStatus
        /// </summary>
        /// <param name="entity">The DeprivationStatus</param>        
        Tuple<bool, DeprivationStatusEntity> Edit(T entity);

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
        /// <param name="DeprivationStatusCode">The DeprivationStatus</param>
        /// <returns>The DeprivationStatus</returns>
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
        /// <returns>The DeprivationStatus meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the DeprivationStatus By the spanish o english name
        /// </summary>
        /// <param name="DeprivationStatusDesSpanish">The DeprivationStatus name spanish</param>
        /// <param name="DeprivationStatusDesEnglish">The DeprivationStatus name english</param>
        /// <returns>The DeprivationStatus </returns>
        T ListByNames(string DeprivationStatusDesSpanish, string DeprivationStatusDesEnglish);
    }
}

