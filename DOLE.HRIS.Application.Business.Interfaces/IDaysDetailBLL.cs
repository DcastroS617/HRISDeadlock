using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDaysDetailBLL<T> where T : DaysDetailEntity
    {
        /// <summary>
        /// Get list the Days Detail
        /// </summary>    
        /// <param name="daysDetailCode">days Detail Code</param>
        /// <param name="dayTypeCode">day Type Code</param>
        /// <param name="codeDateApplies">code Date Applies</param>
        /// <param name="codeDateBase">code Date Base</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        PageHelper<T> GetDaysDetailList(string geographicDivisionCode, int divisionCode, int daysDetailCode, int dayTypeCode, string descriptionDay, DateTime? codeDateBase, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        /// <returns>DaysDetailEntity</returns>
        T GetDaysDetailByDaysDetailCode(int daysDetailCode);

        /// <summary>
        /// Get Days Detail By Date  Apllies
        /// </summary>        
        /// <param name="daysDetailEntity">Days Detail Entity</param> 
        /// <returns> A List of DaysDetailEntity</returns>
        List<T> GetDaysDetailByDate(DaysDetailEntity daysDetailEntity);

        /// <summary>
        /// Save the Days Detail
        /// </summary>
        /// <param name="daysDetailEntity">days Detail Entity</param> 
        bool AddDaysDetail(DaysDetailEntity daysDetail);

        /// <summary>
        /// Update the Days Detail
        /// </summary>
        /// <param name="daysDetailEntity">days Detail Entity</param> 
        bool UpdateDaysDetail(DaysDetailEntity daysDetail);

        /// <summary>
        /// Delete the Days Detail
        /// </summary>
        /// <param name="daysDetailEntity">days Detail Entity</param> 
        bool DeleteDaysDetail(int daysDetailCode);
    }
}
