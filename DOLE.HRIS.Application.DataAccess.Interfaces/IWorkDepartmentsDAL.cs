using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWorkDepartmentsDAL<T> where T : WorkDepartmentsEntity
    {
        /// <summary>
        /// Get a Work Deparments List
        /// </summary>
        /// <param name="workDepartmentCode">workDepartmentCode</param>
        PageHelper<T> GetWorkDepartmentsList(string geographicDivisionCode, int divisionCode, int workDepartmentCode, int overtimeCompanieCode, string workDepartmentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get a Work Deparments entity by workDepartmentCode
        /// </summary>
        /// <param name="workDepartmentCode">workDepartmentCode</param>
        T GetWorkDepartmentsByWorkDepartmentCode(int workDepartmentCode);

        /// <summary>
        /// Add a Work Deparments
        /// </summary>
        /// <param name="workDepartmentCode">workDepartmentCode</param>
        bool AddWorkDepartments(WorkDepartmentsEntity workDepartments);

        /// <summary>
        /// Delete a Work Deparments
        /// </summary>
        /// <param name="workDepartmentCode">workDepartmentCode</param>
        bool UpdateWorkDepartments(WorkDepartmentsEntity workDepartments);

        /// <summary>
        /// Delete a Work Deparments
        /// </summary>
        /// <param name="workDepartmentCode">workDepartmentCode</param>
        bool DeleteWorkDepartments(int workDepartmentCode);
    }
}
