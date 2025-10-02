using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IReasonNotWorkBLL<T> where T : ReasonNotWorkEntity
    {
        /// <summary>
        /// List the Academic Degrees enabled
        /// </summary>
        /// <returns>The Academic Degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the Principal Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        Tuple<bool, ReasonNotWorkEntity> Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotWork By key
        /// </summary>
        /// <param name="ReasonNotWorkCode">The ReasonNotWork code</param>
        /// <returns>The ReasonNotWork </returns>
        T ListByKey(short ReasonNotWorkCode);

        /// <summary>
        /// List the ReasonNotWork by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotWorkNameSpanish">The ReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">The ReasonNotWork name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The ReasonNotWork meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the ReasonNotWork By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotWorkNameSpanish">The ReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">The ReasonNotWork name english</param>
        /// <returns>The ReasonNotWork </returns>
        T ListByNames(string ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish);
    }
}
