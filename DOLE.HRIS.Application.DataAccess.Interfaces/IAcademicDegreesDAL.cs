using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAcademicDegreesDal<T> where T : AcademicDegreeEntity
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        byte Add(T entity);

        /// <summary>
        /// Edit the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        void Edit(T entity);

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
        PageHelper<T> ListByFilters(int divisionCode, string AcademicDegreeNameSpanish, string AcademicDegreeNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the academic degrees By the spanish o english name
        /// </summary>
        /// <param name="academic degreesNameSpanish">The academic degrees name spanish</param>
        /// <param name="academic degreesNameEnglish">The academic degrees name english</param>
        /// <returns>The academic degrees </returns>
        T ListByNames(string AcademicDegreeNameSpanish, string AcademicDegreeNameEnglish);
    }
}