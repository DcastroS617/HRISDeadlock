using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IOvertimeApprovalTypesBLL<T> where T : OvertimeApprovalTypesEntity
    {
        /// <summary>
        /// List the OvertimeApprovalTypes
        /// </summary>        
        /// <returns>The Overtime Approval Types List</returns>
        PageHelper<T> GetOvertimeApprovalTypesList(string geographicDivisionCode, int divisionCode, int overtimeApprovalTypeCode, string overtimeApprovalTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// Get OvertimeApprovalTypes Record By OvertimeApprovalTypeCode
        /// </summary>        
        /// <param name="overtimeApprovalTypeCode">Overtime Companies Code</param> 
        T OvertimeApprovalTypesByOvertimeApprovalTypeCode(int overtimeApprovalTypeCode);

        /// <summary>
        /// Save the OvertimeApprovalTypes
        /// </summary>
        /// <param name="overtimeApprovalTypes">Overtime Approval Types</param> 
        bool AddOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes);

        /// <summary>
        /// Update the OvertimeApprovalTypes
        /// </summary>
        /// <param name="overtimeApprovalTypes">Overtime Approval Types</param>  
        bool UpdateOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes);

        /// <summary>
        /// Delete an OvertimeApprovalType
        /// </summary>
        /// <param name="overtimeApprovalTypeCode">OvertimeApprovalTypeCode</param>
        bool DeleteOvertimeApprovalTypes(int overtimeApprovalTypeCode);
    }
}
