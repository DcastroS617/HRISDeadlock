using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IReasonNotMedicalDal<T> where T : ReasonNotMedicalEntity
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the ReasonNotMedicalCode
        /// </summary>
        /// <param name="entity">The ReasonNotMedicalCode</param>
        short Add(T entity);

        /// <summary>
        /// Edit ReasonNotMedicalCode
        /// </summary>
        /// <param name="entity">The ReasonNotMedicalCode</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotMedicalCode
        /// </summary>
        /// <param name="entity">The ReasonNotMedicalCode</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotMedicalCode
        /// </summary>
        /// <param name="entity">The ReasonNotMedicalCode</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotMedicalCode By key
        /// </summary>
        /// <param name="ReasonNotMedicalCode">The ReasonNotMedical code</param>
        /// <returns>The ReasonNotMedicalCode </returns>
        T ListByKey(short ReasonNotMedicalCode);

        /// <summary>
        /// List the ReasonNotMedicalCode by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotMedicalNameSpanish">TheReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">The ReasonNotMedical name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Principal Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the ReasonNotMedicalCoden By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotMedicalNameSpanish">The ReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">The ReasonNotMedical name  english</param>
        /// <returns>The ReasonNotMedical </returns>
        T ListByNames(string ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish);
    }
}
