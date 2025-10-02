using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IRolesByDepartmentEmployeeBLL<T> where T : RolesByDepartmentEmployeeEntity
    {
        /// <summary>
        /// Get List the Roles By Department Employee
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The RolesByDepartmentEmployeeEntity List</return>
        PageHelper<T> GetRolesByDepartmentEmployeeList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Roles By Department Employee By Id
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">roles By Department Employee ID</param>
        /// <returns>RolesByDepartmentEmployeeEntity</returns>
        T GetRolesByDepartmentEmployeeById(int rolesByDepartmentEmployeeID);

        /// <summary>
        /// Save the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param>
        bool AddRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Update the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param>
        bool UpdateRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Delete the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param>
        bool DeleteRolesByDepartmentEmployee(int rolesByDepartmentEmployeeID);

        /// <summary>
        /// Get Role Approvers List For Dropdown
        /// </summary>        
        /// <returns>The RoleApproversEntity List</returns>
        List<RoleApproversEntity> GetRoleApproversListForDropdown();

        /// <summary>
        /// Get Roles By Department Employee By Criteria
        /// </summary>        
        /// <param name="criteria">criteria</param>
        /// <returns>The RolesByDepartmentEmployeeEntity List</returns>
        List<T> GetRolesByDepartmentEmployeeList(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Get Department List For Dropdown
        /// </summary>        
        /// <returns>The Departments List</returns>
        List<Departments> GetDepartmentListForDropdown();
    }
}
