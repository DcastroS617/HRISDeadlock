using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Entity.ADAM;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAbsenteeismAttachedDocumentsDal<T> where T : AbsenteeismAttachedDocumentEntity
    {

        /// <summary>
        /// List the attached documents by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="AttachedDocumentCode">Code</param>
        /// <param name="AttachedDocumentName">Name</param>
        ///  <param name="divisionCodeFilter">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The attached documents meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string AttachedDocumentCode, string AttachedDocumentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue, int deleted = 0);

        /// <summary>
        /// List the attached document by key: Code
        /// </summary>        
        /// <param name="AttachedDocumentCode">attached document code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>The attached document</returns>
        T ListByKey(string attachedDocumentCode);

        /// <summary>
        /// List the first absenteeism attached document by key or name: Code Or Name
        /// </summary>        
        /// <param name="AttachedDocumentCode">Absenteeism attached document code</param>
        ///  <param name="divisionCode">Division Code</param>
        /// <param name="AttachedDocumentName">attached document name</param>
        /// <returns>The absenteeismattached document</returns>
        T ListByKeyOrName(string AttachedDocumentCode, int divisionCode, string AttachedDocumentName);

        /// <summary>
        /// Add the attached document
        /// </summary>
        /// <param name="entity">The attached document</param>
        AbsenteeismAttachedDocumentEntity Add(T entity);

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

        void AddDocumentByDivision(AbsenteeismAttachedDocumentByDivisionEntity absenteeismDocumentByDivision);
    }
}