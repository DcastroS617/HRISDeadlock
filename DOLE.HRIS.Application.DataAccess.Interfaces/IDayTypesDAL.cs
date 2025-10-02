using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDayTypesDAL<T> where T : DayTypesEntity
    {
        /// <summary>
        /// List the DayTypes
        /// </summary>        
        /// <returns>The DayTypes List</returns>
        PageHelper<T> GetDayTypesList(string geographicDivisionCode, int divisionCode, int dayTypeCode, string dayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        /// <summary>
        /// Get DayTypes By DayType Code
        /// </summary>        
        /// <param name="dayTypeCode">DayType Code</param> 
        T GetDayTypesByDayTypeCode(int dayTypeCode);

        /// <summary>
        /// Save the DayTypes
        /// </summary>
        /// <param name="dayTypes">DayTypes</param>  
        bool AddDayTypes(DayTypesEntity dayTypes);

        /// <summary>
        /// Update the DayType
        /// </summary>
        /// <param name="dayTypes">DayTypes</param>   
        bool UpdateDayType(DayTypesEntity dayTypes);

        /// <summary>
        /// Delete a DayType
        /// </summary>
        /// <param name="dayTypeCode">Day Type Code</param>
        bool DeleteDayType(int dayTypeCode);

        /// <summary>
        /// List the DayTypes
        /// </summary>        
        /// <returns>The DayTypes List</returns>
        List<T> GetDayTypesListForDropdown(string geographicDivisionCode, int divisionCode);
    }
}
