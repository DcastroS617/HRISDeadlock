using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IProfessionsBll<T> where T : ProfessionEntity
    {
        /// <summary>
        /// List the Professions enabled
        /// </summary>
        /// <returns>The Professions</returns>
        List<T> ListEnabled();

        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        /// <returns>Tuple: En the first item a bool: true if Profession successfully added. False otherwise
        /// Second item: the Profession added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>        
        Tuple<bool, ProfessionEntity> Edit(T entity);

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
        /// <param name="professionCode">The Profession</param>
        /// <returns>The Profession</returns>
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
        /// <returns>The Profession meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string professionNameSpanish, string professionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// List the Profession By the spanish o english name
        /// </summary>
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        T ListByNames(string professionNameSpanish, string professionNameEnglish);
    }
}