using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public  interface ITypeTrainingBll
    {
        PageHelper<TypeTrainingEntity> TypeTrainingListByFilter(TypeTrainingEntity typeTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber);

        TypeTrainingEntity TypeTrainingByKey(TypeTrainingEntity typeTraining);

        TypeTrainingEntity TypeTrainingAdd(TypeTrainingEntity typeTraining);

        TypeTrainingEntity TypeTrainingEdit(TypeTrainingEntity typeTraining);

        ListItem[] TypeTrainingListByCatalog();
    }
}
