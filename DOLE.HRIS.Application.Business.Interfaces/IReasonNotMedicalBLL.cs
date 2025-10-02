using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IReasonNotMedicalBLL<T> where T : ReasonNotMedicalEntity
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
        /// Edit the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        Tuple<bool, ReasonNotMedicalEntity> Edit(T entity);

        /// <summary>
        /// Delete the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        void Activate(T entity);

        /// <summary>
        /// List the ReasonNotMedical By key
        /// </summary>
        /// <param name="ReasonNotMedicalCode">The ReasonNotMedical code</param>
        /// <returns>The ReasonNotMedical </returns>
        T ListByKey(short ReasonNotMedicalCode);

        /// <summary>
        /// List the ReasonNotMedical by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotMedicalNameSpanish">The ReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">The ReasonNotMedical name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The ReasonNotMedical meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the ReasonNotMedical By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotMedicalNameSpanish">The ReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalNameEnglish">The ReasonNotMedical name english</param>
        /// <returns>The ReasonNotMedical </returns>
        T ListByNames(string ReasonNotMedicalNameSpanish, string ReasonNotMedicalNameEnglish);
    }
}
