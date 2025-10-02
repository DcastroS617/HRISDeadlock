using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWorkingDayTypesBLL<T> where T : WorkingDayTypesEntity
    {
        /// <summary>
        /// List the WorkingDayTypes
        /// </summary>        
        /// <returns>The WorkingDayTypes List</returns>
        PageHelper<T> GetWorkingDayTypesList(string geographicDivisionCode, int divisionCode, int workingDayTypeCode, string workingDayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get WorkingDayTypes Record By WorkingDayTypeCode
        /// </summary>        
        /// <param name="workingDayTypeCode">WorkingDayTypeCode</param> 
        T WorkingDayTypesByWorkingDayTypeCode(int workingDayTypeCode);

        /// <summary>
        /// Save the WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param> 
        bool AddWorkingDayTypes(WorkingDayTypesEntity workingDayTypes);

        /// <summary>
        /// Update the WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param>  
        bool UpdateWorkingDayTypes(WorkingDayTypesEntity workingDayTypes);

        /// <summary>
        /// Delete a WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypeCode">OvertimeApprovalTypeCode</param>
        bool DeleteWorkingDayTypes(int workingDayTypeCode);
    }
}
