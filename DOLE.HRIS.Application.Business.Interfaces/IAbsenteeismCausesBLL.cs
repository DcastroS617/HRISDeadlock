using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismCausesBll<T> where T : AbsenteeismCauseEntity
    {
        /// <summary>
        /// List the causes by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="causeCode">Code</param>
        /// <param name="causeName">Name</param>
        /// <param name="causeCategoryCode">Cause category code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The causes meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string causeCode, string causeName, string causeCategoryCode, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the causes by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The causes meeting the given filters</returns>
        List<AbsenteeismCauseEntity> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the cause by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="causeCode">Cause code</param>
        /// <returns>The cause</returns>
        T ListByKey(string geographicDivisionCode, string causeCode, int? DivisionCode);

        /// <summary>
        /// Add the cause
        /// </summary>
        /// <param name="entity">The cause</param>
        Tuple<bool, AbsenteeismCauseEntity> Add(T entity);

        /// <summary>
        /// Edit the cause
        /// </summary>
        /// <param name="entity">The cause</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the cause
        /// </summary>
        /// <param name="entity">The cause</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the cause
        /// </summary>
        /// <param name="entity">The cause</param>
        void Activate(T entity);

        /// <summary>
        /// List the Causes Categories
        /// </summary>
        /// <returns>The Causes categories</returns>
        List<AbsenteeismCauseCategoryEntity> ListCauseCategories();

        /// <summary>
        /// Get Cause Information to export
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns></returns>
        AbsenteeismCauseInformationEntity CauseInformation(int divisionCode);
    }
}