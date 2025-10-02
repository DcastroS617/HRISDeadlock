using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IEmployeeTaskDal
    {
        DbaEntity EmployeeTaskAdd(EmployeeTaskEntity entity);
        
        DbaEntity EmployeeTaskDesactivate(EmployeeTaskEntity entity);
        
        EmployeeTaskEntity EmployeeTaskDetail(EmployeeTaskEntity entity);
       
        ListItem[] EmployeeTaskListByEnabled();
        
        DbaEntity EmployeeTaskEdit(EmployeeTaskEntity entity);
        
        PageHelper<EmployeeTaskEntity> EmployeeTaskListByFilter(EmployeeTaskEntity entity, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
        
        List<EmployeeTaskEntity> ListEnabled();
    }
}