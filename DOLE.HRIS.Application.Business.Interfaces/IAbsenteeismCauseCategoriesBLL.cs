using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismCauseCategoriesBll<T> where T : AbsenteeismCauseCategoryEntity
    {
        /// <summary>
        /// List the cause categories by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="causeCategoryCode">Code</param>
        /// <param name="causeCategoryName">Name</param>        
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The cause categories meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string causeCategoryCode, string causeCategoryName, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the cause category by key: Code
        /// </summary>        
        /// <param name="causeCategoryCode">Cause Category code</param>
        /// <returns>The cause Category</returns>
        T ListByKey(string causeCategoryCode);

        /// <summary>
        /// Add the cause Category
        /// </summary>
        /// <param name="entity">The cause Category</param>
        Tuple<bool, AbsenteeismCauseCategoryEntity> Add(T entity);

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
    }
}