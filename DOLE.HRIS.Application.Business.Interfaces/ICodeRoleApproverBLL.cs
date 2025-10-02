using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    /// <summary>
    /// Interface for business logic related to CodeRoleApprover entities.
    /// Defines operations for retrieving, adding, updating, and deleting CodeRoleApprover records.
    /// </summary>
    /// <typeparam name="T">Type that inherits from CodeRoleApproverEntity.</typeparam>
    public interface ICodeRoleApproverBLL<T> where T : CodeRoleApproverEntity
    {
        /// <summary>
        /// Retrieves a paginated list of CodeRoleApprover entities based on filter and sorting parameters.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code to filter the records.</param>
        /// <param name="divisionCode">Division code to filter the records.</param>
        /// <param name="codeRoleApproverID">Specific CodeRoleApprover ID to search for.</param>
        /// <param name="roleApprover">Role approver name or keyword to filter.</param>
        /// <param name="sortExpression">Field by which to sort the results.</param>
        /// <param name="sortDirection">Direction of sorting (e.g., ASC or DESC).</param>
        /// <param name="pageNumber">Current page number for pagination.</param>
        /// <param name="pageSize">Optional size of the page. If null, a default may be used.</param>
        /// <returns>A PageHelper object containing the list of matching CodeRoleApprover records.</returns>
        PageHelper<T> GetCodeRoleApproverList(string geographicDivisionCode, int divisionCode, int codeRoleApproverID, string roleApprover, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Retrieves a specific CodeRoleApprover entity by its ID.
        /// </summary>
        /// <param name="codeRoleApproverID">The unique identifier of the CodeRoleApprover.</param>
        /// <returns>The CodeRoleApprover entity if found; otherwise, null.</returns>
        T GetCodeRoleApproverByCodeRoleApproverID(int codeRoleApproverID);

        /// <summary>
        /// Adds a new CodeRoleApprover entity to the system.
        /// </summary>
        /// <param name="codeRoleApprover">The entity to be added.</param>
        /// <returns>A tuple indicating whether the operation was successful and the ID of the newly created entity.</returns>
        Tuple<bool, int> AddCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover);

        /// <summary>
        /// Updates an existing CodeRoleApprover entity in the system.
        /// </summary>
        /// <param name="codeRoleApprover">The entity with updated values.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        bool UpdateCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover);

        /// <summary>
        /// Deletes a specific CodeRoleApprover entity by its ID.
        /// </summary>
        /// <param name="codeRoleApproverID">The unique identifier of the entity to be deleted.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        bool DeleteCodeRoleApprover(int codeRoleApproverID);

        /// <summary>
        /// Retrieves a list of CodeRoleApprover entities for use in dropdown UI components.
        /// </summary>
        /// <returns>A list of CodeRoleApprover entities with minimal data suitable for display in dropdowns.</returns>
        List<T> GetCodeRoleApproverListForDropdown();
    }
}
