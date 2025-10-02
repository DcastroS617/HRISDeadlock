using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IEmployeeWorkBll
    {
        DbaEntity EmployeeWorkAdd(EmployeeWorksEntity entity);
        
        DbaEntity EmployeeWorkDesactivate(EmployeeWorksEntity entity);
        
        EmployeeWorksEntity EmployeeWorkDetail(EmployeeWorksEntity entity);
        
        ListItem[] EmployeeWorkListByEnable();
        
        DbaEntity EmployeeWorkEdit(EmployeeWorksEntity entity);
        
        PageHelper<EmployeeWorksEntity> EmployeeWorksListByFilter(EmployeeWorksEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber);
    }
}