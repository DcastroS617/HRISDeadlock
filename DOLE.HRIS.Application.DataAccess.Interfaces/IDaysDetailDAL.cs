using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDaysDetailDAL<T> where T : DaysDetailEntity
    {
        /// <summary>
        /// List the Days Detail
        /// </summary>        
        /// <returns>The Days Detail List</returns>
        PageHelper<T> GetDaysDetailList(string geographicDivisionCode, int divisionCode, int daysDetailCode, int dayTypeCode, string descriptionDay, DateTime? codeDateBase, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        T GetDaysDetailByDaysDetailCode(int daysDetailCode);

        /// <summary>
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        List<T> GetDaysDetailByDate(DaysDetailEntity daysDetailEntity);

        /// <summary>
        /// Save the DaysDetail
        /// </summary>
        /// <param name="daysDetail">DaysDetail</param> 
        bool AddDaysDetail(DaysDetailEntity daysDetail);

        /// <summary>
        /// Update the DaysDetail
        /// </summary>
        /// <param name="daysDetail">DaysDetail</param>
        bool UpdateDaysDetail(DaysDetailEntity daysDetail);

        /// <summary>
        /// Delete the DaysDetail
        /// </summary>
        /// <param name="daysDetail">DaysDetail</param>
        bool DeleteDaysDetail(int daysDetailCode);
    }
}
