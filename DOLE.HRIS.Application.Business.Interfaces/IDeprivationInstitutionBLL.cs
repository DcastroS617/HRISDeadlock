using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDeprivationInstitutionBLL<T> where T : DeprivationInstitutionEntity
    {
        List<T> ListAll();

        /// <summary>
        /// List the DeprivationInstitution enabled
        /// </summary>
        /// <returns>The DeprivationInstitution</returns>
        List<T> ListEnabled();

        /// Add the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        /// <returns>Tuple: En the first item a bool: true if DeprivationInstitution successfully added. False otherwise
        /// Second item: the DeprivationInstitution added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Edit the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>        
        Tuple<bool, DeprivationInstitutionEntity> Edit(T entity);

        /// <summary>
        /// Delete the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        void Activate(T entity);

        /// <summary>
        /// List the DeprivationInstitution By key
        /// </summary>
        /// <param name="DeprivationInstitutionCode">The DeprivationInstitution</param>
        /// <returns>The DeprivationInstitution</returns>
        T ListByKey(short DeprivationInstitutionCode);

        /// <summary>
        /// List the DeprivationInstitution by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="DeprivationInstitutionDesSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionDesEnglish">The DeprivationInstitution name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The DeprivationInstitution meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// List the DeprivationInstitution By the spanish o english name
        /// </summary>
        /// <param name="DeprivationInstitutionDesSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionDesEnglish">The DeprivationInstitution name english</param>
        /// <returns>The DeprivationInstitution </returns>
        T ListByNames(string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish);
    }
}

