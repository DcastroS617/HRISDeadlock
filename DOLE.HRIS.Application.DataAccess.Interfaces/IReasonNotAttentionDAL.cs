using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IReasonNotAttentionDal<T> where T : ReasonNotAttentionEntity
    {
        /// <summary>
        /// List the Reason NotAttention Code enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the ReasonNotAttentionCode
        /// </summary>
        /// <param name="entity">The ReasonNotAttentionCode</param>
        short Add(T entity);

        /// <summary>
        /// Edit ReasonNotAttentionCode
        /// </summary>
        /// <param name="entity">The ReasonNotAttentionCode</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotAttentionCode
        /// </summary>
        /// <param name="entity">The ReasonNotAttentionCode</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotAttentionCode
        /// </summary>
        /// <param name="entity">The ReasonNotAttentionCode</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotAttentionCode By key
        /// </summary>
        /// <param name="ReasonNotAttentionCode">The Principal Profession code</param>
        /// <returns>The ReasonNotAttentionCode </returns>
        T ListByKey(short ReasonNotAttentionCode);

        /// <summary>
        /// List the ReasonNotAttentionCode by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotAttentionNameSpanish">The Principal Profession name spanish</param>
        /// <param name="ReasonNotAttentionNameEnglish">The Principal Profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Principal Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the ReasonNotAttentionCoden By the spanish o english name
        /// </summary>
        /// <param name="Principal ProfessionNameSpanish">The ReasonNotAttentionCode name spanish</param>
        /// <param name="Principal ProfessionNameEnglish">The ReasonNotAttentionCode name  english</param>
        /// <returns>The Principal Profession </returns>
        T ListByNames(string ReasonNotAttentionNameSpanish, string ReasonNotAttentionNameEnglish);
    }
}
