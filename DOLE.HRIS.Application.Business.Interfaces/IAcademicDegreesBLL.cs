using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAcademicDegreesBll<T> where T : AcademicDegreeEntity
    {
        /// <summary>
        /// List the Academic Degrees enabled
        /// </summary>
        /// <returns>The Academic Degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>

        /// <summary>
        /// Add the Principal Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        Tuple<bool, AcademicDegreeEntity> Edit(T entity);
        /// <summary>
        /// Delete the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        void Activate(T entity);

        /// <summary>
        /// List the academic degrees By key
        /// </summary>
        /// <param name="academic degreesCode">The academic degrees code</param>
        /// <returns>The academic degrees </returns>
        T ListByKey(short AcademicDegreeCode);

        /// <summary>
        /// List the academic degrees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="academic degreesNameSpanish">The academic degrees name spanish</param>
        /// <param name="academic degreesNameEnglish">The academic degrees name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The academic degrees meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string AcademicDegreeNameSpanish, string AcademicDegreeNameEnglish, string sortExpression, string sortDirection, int pageNumber);

        /// <summary>
        /// List the academic degrees By the spanish o english name
        /// </summary>
        /// <param name="academic degreesNameSpanish">The academic degrees name spanish</param>
        /// <param name="academic degreesNameEnglish">The academic degrees name english</param>
        /// <returns>The academic degrees </returns>
        T ListByNames(string AcademicDegreeNameSpanish, string AcademicDegreeNameEnglish);
    }
}