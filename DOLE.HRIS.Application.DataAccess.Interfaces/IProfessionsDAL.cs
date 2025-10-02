using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IProfessionsDal<T> where T : ProfessionEntity
    {
        /// <summary>
        /// List the Professions enabled
        /// </summary>
        /// <returns>The Professions</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        short Add(T entity);

        /// <summary>
        /// Edit the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        void Activate(T entity);

        /// <summary>
        /// List the Profession By key
        /// </summary>
        /// <param name="professionCode">The Profession code</param>
        /// <returns>The Profession </returns>
        T ListByKey(short professionCode);

        /// <summary>
        /// List the Profession by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string professionNameSpanish, string professionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
        
        /// <summary>
        /// List the Profession By the spanish o english name
        /// </summary>
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        T ListByNames(string professionNameSpanish, string professionNameEnglish);
    }
}