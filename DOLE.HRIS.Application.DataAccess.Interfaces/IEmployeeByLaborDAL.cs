using System.Collections.Generic;
using DOLE.HRIS.Shared.Entity;
using System.Data;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IEmployeeByLaborDal
    {
        DbaEntity EmployeeByLaborAdd(EmployeeByLaborEntity entity, DataTable EMPLOYEES);
        
        DbaEntity EmployeeByLaborDelete(EmployeeByLaborEntity entity);
        
        List<EmployeeByLaborEntity> EmployeeByLaborWithFilter(EmployeeByLaborEntity entity, string SortExpression, string SortDirection);
       
        ListItem[] CompaniesListEnableByDivision(int DivisionCode);
        
        ListItem[] NominalClassListEnableByDivision(string GeographicDivisionID, int? CompanyID);
        
        ListItem[] CostCenterListEnableByDivision(string GeographicDivisionCode, int? CompanyID, string PayrollClassCode);
        
        ListItem[] PositionsListEnabled(int? CompanyCode, string PayrollClassCode);
    }
}