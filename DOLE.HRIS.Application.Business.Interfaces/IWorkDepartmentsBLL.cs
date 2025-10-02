using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWorkDepartmentsBLL<T> where T : WorkDepartmentsEntity
    {
        /// <summary>
        /// Get list the WorkDepartments
        /// </summary>   
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="userCodeID">user Code ID</param>
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The WorkDepartmentsEntity List</returns>
        PageHelper<T> GetWorkDepartmentsList(string geographicDivisionCode, int divisionCode, int workDepartmentCode, int overtimeCompanieCode, string workDepartmentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Work Departments By Work Department Code
        /// </summary>        
        /// <param name="workDepartmentCode">Work Department Code</param> 
        T GetWorkDepartmentsByWorkDepartmentCode(int workDepartmentCode);

        /// <summary>
        /// Save the Work Departments
        /// </summary>
        /// <param name="workDepartmentsEntity">work Departments Entity</param>
        bool AddWorkDepartments(WorkDepartmentsEntity workDepartments);

        /// <summary>
        /// Update the Work Departments
        /// </summary>
        /// <param name="workDepartmentsEntity">work Departments Entity</param>
        bool UpdateWorkDepartments(WorkDepartmentsEntity workDepartments);

        /// <summary>
        /// Delete the Work Departments
        /// </summary>
        /// <param name="workDepartmentsEntity">work Departments Entity</param>
        bool DeleteWorkDepartments(int workDepartmentCode);
    }
}
