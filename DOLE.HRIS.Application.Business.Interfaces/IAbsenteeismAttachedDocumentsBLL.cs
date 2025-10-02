using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismAttachedDocumentsBll<T> where T : AbsenteeismAttachedDocumentEntity
    {
        /// <summary>
        /// List the attached documents by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="attachedDocumentCode">Code</param>
        /// <param name="attachedDocumentName">Name</param>       
        /// <param name="divisionCodeFilter">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The attached documents meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string attachedDocumentCode, string attachedDocumentName, string sortExpression, string sortDirection, int? pageNumber, int deleted = 0);

        /// <summary>
        /// List the attached document by key: Code
        /// </summary>        
        /// <param name="attachedDocumentCode">attached document code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>The attached document</returns>
        T ListByKey(string attachedDocumentCode);

        /// <summary>
        /// Add the attached document
        /// </summary>
        /// <param name="entity">The attached document</param>
        void Add(T entity);

        /// <summary>
        /// Edit the attached document
        /// </summary>
        /// <param name="entity">The attached document</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the attached document
        /// </summary>
        /// <param name="entity">The attached document</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the attached document
        /// </summary>
        /// <param name="entity">The attached document</param>
        void Activate(T entity);

        /// <summary>
        /// Get Types Of Attached document by division
        /// </summary>
        /// <param name="division">Division code for filter</param>
        List<Document> ListAttachedDocumentTypeByDivision(int division, bool deleted, bool searchEnabled);
    }
}