using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAbsenteeismCauseCategoriesDal<T> where T : AbsenteeismCauseCategoryEntity
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
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The causes meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string causeCategoryCode, string causeCategoryName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the cause by key: Code
        /// </summary>        
        /// <param name="causeCategoryCode">Cause Category code</param>
        /// <returns>The cause Category</returns>
        T ListByKey(string causeCode);

        /// <summary>
        /// List the first absenteeism cause category by key or name: Code Or Name
        /// </summary>        
        /// <param name="causeCategoryCode">Absenteeism cause category code</param>
        /// <param name="causeCategoryName">Cause Category name</param>
        /// <returns>The absenteeismCause</returns>
        T ListByKeyOrName(string causeCategoryCode, string causeCategoryName);

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
    }
}