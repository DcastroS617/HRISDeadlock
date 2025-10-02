using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWorkingTimeHoursByDepartmentEmployeesBLL<T> where T : WorkingTimeHoursByDepartmentEmployeesEntity
    {
        /// <summary>
        /// Get List the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeHoursByDepartmentEmployeesEntity List</return>
        PageHelper<T> GetWorkingTimeHoursByDepartmentEmployeesList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Working Time Hours By Department Employees By Id
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">working TimeHours By Department Employees ID</param>
        /// <returns>WorkingTimeHoursByDepartmentEmployeesEntity</returns>
        T GetWorkingTimeHoursByDepartmentEmployeesById(int workingTimeHoursByDepartmentEmployeesID);

        /// <summary>
        /// Save the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesEntity">working TimeHours By Department Employees Entity</param>
        bool AddWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees);

        /// <summary>
        /// Update the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesEntity">working TimeHours By Department Employees Entity</param>
        bool UpdateWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees);

        /// <summary>
        /// Delete the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesEntity">working TimeHours By Department Employees Entity</param>
        bool DeleteWorkingTimeHoursByDepartmentEmployees(int workingTimeHoursByDepartmentEmployeesID);
    }
}
