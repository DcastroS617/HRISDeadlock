using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IPrincipalProfesionBLL<T> where T : PrincipalProfesionEntity
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
        /// Edit the Principal Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        Tuple<bool, PrincipalProfesionEntity> Edit(T entity);

        /// <summary>
        /// Delete the Principal Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the Principal Profession
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        void Activate(T entity);

        /// <summary>
        /// List the Principal Profession By key
        /// </summary>
        /// <param name="Principal ProfessionCode">The Principal Profession code</param>
        /// <returns>The Principal Profession </returns>
        T ListByKey(short PrincipalProfessionCode);

        /// <summary>
        /// List the Principal Profession by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="Principal ProfessionNameSpanish">The Principal Profession name spanish</param>
        /// <param name="Principal ProfessionNameEnglish">The Principal Profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Principal Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string PrincipalProfessionNameSpanish, string PrincipalProfessionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the Principal Profession By the spanish o english name
        /// </summary>
        /// <param name="Principal ProfessionNameSpanish">The Principal Profession name spanish</param>
        /// <param name="Principal ProfessionNameEnglish">The Principal Profession name english</param>
        /// <returns>The Principal Profession </returns>
        T ListByNames(string PrincipalProfessionNameSpanish, string PrincipalProfessionNameEnglish);
    }
}
