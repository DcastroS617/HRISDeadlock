using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IHousingTypesBll<T> where T : HousingTypeEntity
    {
        /// <summary>
        /// List the Housing Types enabled
        /// </summary>
        /// <returns>The Housing Types</returns>
        List<T> ListEnabled();
     
        /// Add the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>
        /// <returns>Tuple: En the first item a bool: true if Housing Type successfully added. False otherwise
        /// Second item: the HousingType added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, T> Add(T entity);
        
        /// <summary>
        /// Edit the Housing Type
        /// </summary>
        /// <param name="entity">The Housing Type</param>        
        Tuple<bool, HousingTypeEntity> Edit(T entity);
      
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
        /// <returns>The Housing Type</returns>
        T ListByKey(byte housingTypeCode);
        
        /// <summary>
        /// List the Housing Type by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="HousingTypeDescriptionSpanish">The HousingType Description spanish</param>
        /// <param name="HousingTypeDescriptionEnglish">The HousingType Description english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The HousingType meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
       
        /// <summary>
        /// List the Housing Type By the spanish o english Description
        /// </summary>
        /// <param name="housingTypeDescriptionSpanish">The Housing Type Description spanish</param>
        /// <param name="housingTypeDescriptionEnglish">The Housing Type Description english</param>
        /// <returns>The HousingType </returns>
        T ListByDescription(string housingTypeDescriptionSpanish, string housingTypeDescriptionEnglish);
    }
}