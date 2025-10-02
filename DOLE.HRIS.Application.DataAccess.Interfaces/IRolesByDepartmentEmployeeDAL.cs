using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IRolesByDepartmentEmployeeDAL<T> where T : RolesByDepartmentEmployeeEntity
    {
        /// <summary>
        /// Get Roles By Department Employee list
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return> A List of RolesDeparmentsDepartments</return> 
        PageHelper<T> GetRolesByDepartmentEmployeeList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Roles by Departmets list by EmployeeID
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">Roles By Department Employee Entity</param> 
        /// <return> A List of Departments</return> 
        T GetRolesByDepartmentEmployeeById(int rolesByDepartmentEmployeeID);

        /// <summary>
        /// Add Roles by Departmets
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return> bool</return> 
        bool AddRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Update Roles by Departmets
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return>bool</return> 
        bool UpdateRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Delete Roles by Departmets
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return> bool</return> 
        bool DeleteRolesByDepartmentEmployee(int rolesByDepartmentEmployeeID);

        /// <summary>
        /// Get Departmets list  for dropDown
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return> A List of Departments</return> 
        List<RoleApproversEntity> GetRoleApproversListForDropdown();

        /// <summary>
        /// Get Departmets list  for dropDown
        /// </summary>        
        /// <param name="RolesByDepartmentEmployeeEntity">Roles By Department Employee Entity</param> 
        /// <return> A List of Departments</return> 
        List<T> GetRolesByDepartmentEmployeeList(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee);

        /// <summary>
        /// Get Departmets list  for dropDown
        /// </summary>        
        /// <return> A List of Departments</return> 
        List<Departments> GetDepartmentListForDropdown();
    }
}
