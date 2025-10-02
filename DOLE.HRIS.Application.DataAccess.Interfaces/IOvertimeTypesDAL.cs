using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface IOvertimeTypesDAL<T> where T : OvertimeTypesEntity
    {

        /// <summary>
        /// Get Overtime types list 
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">Overtime Types Entity</param> 
        /// <return> A List of Overtime types</return> 
        PageHelper<T> GetOvertimeTypesList(string geographicDivisionCode, int divisionCode, int overtimeTypeCode, string overtimeTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Overtime types list by EmployeeID
        /// </summary>        
        /// <param name="overtimeTypeCode">Overtime Types Entity</param> 
        /// <return> A List of Overtime types</return> 
        T GetOvertimeTypesByOvertimeTypeCode(int overtimeTypeCode);

        /// <summary>
        /// Get Overtime types list by EmployeeID
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">Overtime Types Entity</param> 
        /// <return> A List of Overtime types</return> 
        bool AddOvertimeTypes(OvertimeTypesEntity overtimeTypes);

        /// <summary>
        /// Get Overtime types list by EmployeeID
        /// </summary>        
        /// <param name="overtimeTypes">Overtime Types Entity</param> 
        /// <return> A List of Overtime types</return> 
        bool UpdateOvertimeTypes(OvertimeTypesEntity overtimeTypes);

        /// <summary>
        /// Get Overtime types list by EmployeeID
        /// </summary>        
        /// <param name="overtimeTypeCode">Overtime Types Entity</param> 
        /// <return> A List of Overtime types</return> 
        bool DeleteOvertimeTypes(int overtimeTypeCode);

        /// <summary>
        /// Get Overtime types list by EmployeeID
        /// </summary>        
        /// <return> A List of Overtime Types</return> 
        List<T> GetOvertimeTypesListForDropdown();
    }
}
