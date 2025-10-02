using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IReasonNotWorkDal<T> where T : ReasonNotWorkEntity
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the ReasonNotWorkCode
        /// </summary>
        /// <param name="entity">The ReasonNotWorkCode</param>
        short Add(T entity);

        /// <summary>
        /// Edit ReasonNotWorkCode
        /// </summary>
        /// <param name="entity">The ReasonNotWorkCode</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotWorkCode
        /// </summary>
        /// <param name="entity">The ReasonNotWorkCode</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotWorkCode
        /// </summary>
        /// <param name="entity">The ReasonNotWorkCode</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotWorkCode By key
        /// </summary>
        /// <param name="ReasonNotWorkCode">The ReasonNotWork code</param>
        /// <returns>The ReasonNotWorkCode </returns>
        T ListByKey(short ReasonNotWorkCode);

        /// <summary>
        /// List the ReasonNotWorkCode by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotWorkNameSpanish">TheReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">The ReasonNotWork name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Principal Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the ReasonNotWorkCoden By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotWorkNameSpanish">The ReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkNameEnglish">The ReasonNotWork name  english</param>
        /// <returns>The ReasonNotWork </returns>
        T ListByNames(string ReasonNotWorkNameSpanish, string ReasonNotWorkNameEnglish);
    }
}
