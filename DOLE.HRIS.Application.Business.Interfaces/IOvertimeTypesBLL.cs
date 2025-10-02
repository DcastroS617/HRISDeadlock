using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IOvertimeTypesBLL<T> where T : OvertimeTypesEntity
    {
        PageHelper<T> GetOvertimeTypesList(string geographicDivisionCode, int divisionCode, int overtimeTypeCode, string overtimeTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        T GetOvertimeTypesByOvertimeTypeCode(int overtimeTypeCode);
        bool AddOvertimeTypes(OvertimeTypesEntity overtimeTypes);
        bool UpdateOvertimeTypes(OvertimeTypesEntity overtimeTypes);
        bool DeleteOvertimeTypes(int overtimeTypeCode);
        List<T> GetOvertimeTypesListForDropdown();
    }
}
