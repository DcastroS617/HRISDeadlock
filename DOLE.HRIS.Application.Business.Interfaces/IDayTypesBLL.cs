using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDayTypesBLL<T> where T : DayTypesEntity
    {
        /// <summary>
        /// Get list the Day Types
        /// </summary>   
        /// <param name="dayTypeCode">day Type Code</param>
        /// <param name="dayTypesName">day Types Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The DayTypesEntity List</returns>
        PageHelper<T> GetDayTypesList(string geographicDivisionCode, int divisionCode, int dayTypeCode, string dayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Day Types By Day Type Code
        /// </summary>        
        /// <param name="dayTypeCode">Day Type Code</param> 
        /// <returns>DayTypesEntity</returns> 
        T GetDayTypesByDayTypeCode(int dayTypeCode);

        /// <summary>
        /// Save the DayTypes
        /// </summary>
        /// <param name="dayTypesEntity">day Types Entity</param>
        bool AddDayTypes(DayTypesEntity dayTypes);

        /// <summary>
        /// Update the DayTypes
        /// </summary>
        /// <param name="dayTypesEntity">day Types Entity</param>
        bool UpdateDayType(DayTypesEntity dayTypes);

        /// <summary>
        /// Delete the DayTypes
        /// </summary>
        /// <param name="dayTypesEntity">day Types Entity</param>
        bool DeleteDayType(int dayTypeCode);

        /// <summary>
        /// Get list Day Types For Dropdown
        /// </summary>        
        /// <returns>The DayTypesEntity list</returns>
        List<T> GetDayTypesListForDropdown(string geographicDivisionCode, int divisionCode);
    }
}
