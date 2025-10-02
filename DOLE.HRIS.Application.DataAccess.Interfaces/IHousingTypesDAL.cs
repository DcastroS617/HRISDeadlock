using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IHousingTypesDal<T> where T : HousingTypeEntity
    {
        /// <summary>
        /// List the Housing Types enabled
        /// </summary>
        /// <returns>The Housing Types</returns>
        List<T> ListEnabled();

        /// <summary>
        /// Add the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        byte Add(T entity);

        /// <summary>
        /// Edit the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        void Activate(T entity);

        /// <summary>
        /// List the Housing Type By key
        /// </summary>
        /// <param name="housingTypeCode">The Housing Type code</param>
        /// <returns>The Housing Type </returns>
        T ListByKey(byte housingTypeCode);

        /// <summary>
        /// List the Housing Type by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="housingTypeDescriptionSpanish">The Housing Type name spanish</param>
        /// <param name="housingTypeDescriptionEnglish">The Housing Type name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The Housing Type meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
        
        /// <summary>
        /// List the Housing Type By the spanish o english description
        /// </summary>
        /// <param name="housingTypeDescriptionSpanish">The Housing Type description spanish</param>
        /// <param name="housingTypeDescriptionEnglish">The Housing Type description english</param>
        /// <returns>The Housing Type </returns>
        T ListByDescription(string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish);
    }
}