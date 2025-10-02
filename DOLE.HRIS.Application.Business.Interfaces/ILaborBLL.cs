using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Data;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ILaborBll
    {
        ListItem[] LaborList(int Division);

        DbaEntity LaborAdd(LaborEntity entity);

        LaborEntity LaborById(LaborEntity entity);

        DbaEntity LaborDelete(LaborEntity entity);

        DbaEntity LaborEdit(LaborEntity entity);

        ListItem[] LaborRegionalList(string LaborRegionalCode, int DivisionCode);

        DbaEntity LaborRegionalInsert(DataTable entity);

        PageHelper<LaborEntity> LaborListByFilter(LaborEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber);
    }
}
