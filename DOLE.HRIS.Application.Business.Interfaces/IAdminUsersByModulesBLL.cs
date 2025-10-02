using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAdminUsersByModulesBll<T> where T : AdminUserByModuleEntity
    {
        /// <summary>
        /// Check if the user is admin for the module in the division
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="moduleCode">Module code</param>
        /// <returns>True if the user is admin for the module in the division. False otherwise</returns>
        bool IsUserAdmin(string username, int divisionCode, int moduleCode);

        /// <summary>
        /// Activate the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        /// 
        void Activate(short userCode, int divisionCode, byte moduleCode, string lastModifiedUser);

        /// <summary>
        /// Add the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="searchEnabled">Search enabled ?</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        Tuple<bool, T> Add(short userCode, int divisionCode, byte moduleCode, bool searchEnabled, string lastModifiedUser);

        /// <summary>
        /// Edit the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="searchEnabled">Search enabled ?</param>
        /// <param name="deleted">Deleted?</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        /// 
        void Edit(short userCode, int divisionCode, byte moduleCode, bool searchEnabled, bool deleted, string lastModifiedUser);

        /// <summary>
        /// Delete the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        /// 
        void Delete(short userCode, int divisionCode, byte moduleCode, string lastModifiedUser);

        /// <summary>
        /// List the permission of the user for the division and module
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <returns>The permission for the user by division an module</returns>
        T ListByUserDivisionModule(short userCode, int divisionCode, byte moduleCode);

        /// <summary>
        /// List the permission to module for user by the given filters
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="workingDivisionCode">The working division code</param>
        /// <param name="divisionCode">The Division code</param
        /// <param name="moduleCode">The module code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The divisions by module meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(short userCode, int workingDivisionCode, int? divisionCode, byte? moduleCode, string sortExpression, string sortDirection, int pageNumber);
    }
}