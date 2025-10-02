using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWorkingTimeTypesBLL<T> where T : WorkingTimeTypesEntity
    {
        /// <summary>
        /// Get List the Working Time Types
        /// </summary>
        /// <param name="divisionCode">division Code</param>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        /// <param name="workingTimeTypeName">working Time Type Name</param>
        /// <param name="totalWorkingTime">total Working Time</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeTypesEntity List</return>
        PageHelper<T> GetWorkingTimeTypesList(int divisionCode, int workingTimeTypeCode, string workingTimeTypeName, int totalWorkingTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Working Time Types By Working Time Type Code
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        /// <returns>WorkingTimeTypesEntity</returns>
        T GetWorkingTimeTypesByWorkingTimeTypeCode(int workingTimeTypeCode);

        /// <summary>
        /// Save the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypesEntity">Working Time Types Entity</param>
        bool AddWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes);

        /// <summary>
        /// Update the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypesEntity">Working Time Types Entity</param>
        bool UpdateWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes);

        /// <summary>
        /// Delete the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypesEntity">Working Time Types Entity</param>
        bool DeleteWorkingTimeTypes(int workingTimeTypeCode);

        /// <summary>
        /// Get Working Time Types List For Dropdown
        /// </summary>
        /// <returns>The WorkingTimeTypesEntity List</returns>
        List<T> GetWorkingTimeTypesListForDropdown();
    }
}
