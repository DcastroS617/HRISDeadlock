using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWorkingTimeHoursByDepartmentEmployeesDAL<T> where T : WorkingTimeHoursByDepartmentEmployeesEntity
    {
        /// <summary>
        /// Get Working Time Hours By Department Employees List
        /// </summary>
        /// <param name="workingTimeRangeEntity">Working Time Range Entity</param>
        PageHelper<T> GetWorkingTimeHoursByDepartmentEmployeesList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">working Time Hours By Department Employees ID</param>
        T GetWorkingTimeHoursByDepartmentEmployeesById(int workingTimeHoursByDepartmentEmployeesID);

        /// <summary>
        /// Add Working Time Hours By Department Employees
        /// </summary>
        /// <param name="WorkingTimeHoursByDepartmentEmployeesEntity">Working Time Hours Entity</param>
        bool AddWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees);

        /// <summary>
        /// Update Working Time Hours By Department Employees
        /// </summary>
        /// <param name="WorkingTimeHoursByDepartmentEmployeesEntity">Working Time Hours Entity</param>
        bool UpdateWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees);

        /// <summary>
        /// Delete Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">Working Time Hours Entity</param>
        bool DeleteWorkingTimeHoursByDepartmentEmployees(int workingTimeHoursByDepartmentEmployeesID); 
    }
}
