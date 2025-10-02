using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDeprivationInstitutionDAL<T> where T : DeprivationInstitutionEntity
    {
        /// <summary>
        /// List the DeprivationInstitution enabled
        /// </summary>
        /// <returns>The Professions</returns>
        List<T> ListEnabled();

        List<T> ListAll();

        /// <summary>
        /// Add the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        short Add(T entity);

        /// <summary>
        /// Edit the DeprivationInstitution
        /// </summary>
        /// <param name="entity">The DeprivationInstitution</param>
        void Edit(T entity);

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
        /// <param name="DeprivationInstitutionCode">The DeprivationInstitution code</param>
        /// <returns>The DeprivationInstitution </returns>
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
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The DeprivationInstitution meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the DeprivationInstitution By the spanish o english name
        /// </summary>
        /// <param name="DeprivationInstitutionDesSpanish">The DeprivationInstitution name spanish</param>
        /// <param name="DeprivationInstitutionDesEnglish">The DeprivationInstitution name english</param>
        /// <returns>The DeprivationInstitution </returns>
        T ListByNames(string DeprivationInstitutionDesSpanish, string DeprivationInstitutionDesEnglish);
    }
}

