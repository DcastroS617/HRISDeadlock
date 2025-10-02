using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWorkingTimeTypesDAL<T> where T : WorkingTimeTypesEntity
    {
        /// <summary>
        /// List the WorkingTimeTypes
        /// </summary>
        /// <return>The WorkingTimeTypes List</return>
        PageHelper<T> GetWorkingTimeTypesList(int divisionCode, int workingTimeTypeCode, string workingTimeTypeName, int totalWorkingTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get WorkingTimeTypes By WorkingTimeTypeCode
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        T GetWorkingTimeTypesByWorkingTimeTypeCode(int workingTimeTypeCode);

        /// <summary>
        /// Save the WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypes">WorkingTimeTypes</param>
        bool AddWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes);

        /// <summary>
        /// Update the WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypes">WorkingTimeTypes</param>
        bool UpdateWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes);

        /// <summary>
        /// Delete a WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        bool DeleteWorkingTimeTypes(int workingTimeTypeCode);
        List<T> GetWorkingTimeTypesListForDropdown();
    }
}
