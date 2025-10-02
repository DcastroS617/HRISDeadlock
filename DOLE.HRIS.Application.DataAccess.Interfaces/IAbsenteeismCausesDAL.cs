using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAbsenteeismCausesDal<T> where T : AbsenteeismCauseEntity
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
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The causes meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string causeCode, string causeName, string causeCategoryCode,  string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

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
        /// List the first absenteeismCause by key or name: GeographicDivisionCode and Code Or Name
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="absenteeismCauseCode">AbsenteeismCause code</param>
        /// <param name="DivisionCode">Division code</param>
        /// <param name="absenteeismCauseName">Cause name</param>
        /// <returns>The absenteeismCause</returns>
        T ListByKeyOrName(string geographicDivisionCode, string absenteeismCauseCode, int? DivisionCode, string absenteeismCauseName);

        /// <summary>
        /// Add the cause
        /// </summary>
        /// <param name="entity">The cause</param>
        void Add(T entity);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        void AddInterestGroupCode(AbsenteeismCauseByDivisionEntity entity);
    }
}