using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
   public interface IReasonNotAttentionBLL<T> where T : ReasonNotAttentionEntity
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
        /// Edit the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        Tuple<bool, ReasonNotAttentionEntity> Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotAttention By key
        /// </summary>
        /// <param name="ReasonNotAttentionCode">The ReasonNotAttention code</param>
        /// <returns>The ReasonNotAttention </returns>
        T ListByKey(short ReasonNotAttentionCode);

        /// <summary>
        /// List the ReasonNotAttention by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotAttentionNameSpanish">The ReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionNameEnglish">The ReasonNotAttention name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The ReasonNotAttention meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the ReasonNotAttention By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotAttentionNameSpanish">The ReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionNameEnglish">The ReasonNotAttention name english</param>
        /// <returns>The ReasonNotAttention </returns>
        T ListByNames(string ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish);
    }
}
