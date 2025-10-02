using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IOvertimeClassificationBLL<T> where T : OvertimeClassificationEntity
    {
        /// <summary>
        /// Get list the Overtime Classification
        /// </summary>    
        /// <param name="overtimeClassification">overtime Classification</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The OvertimeClassificationEntity List</returns>
        PageHelper<T> GetOvertimeClassificationList(int daytype, string sortExpression, string sortDirection, int pageNumber, int? pageSize, OvertimeClassificationEntity overtimeClassification);
        /// <summary>
        /// Get Overtime Classification By Code
        /// </summary>        
        /// <param name="overtimeClassificationCode">Overtime Classification Code</param> 
        /// <returns>OvertimeClassificationEntity</returns>
        T GetOvertimeClassificationByCode(int overtimeClassificationCode);

        /// <summary>
        /// Save the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationEntity">overtime Classification Entity</param> 
        bool AddOvertimeClassification(string geographicDivisionCode, int divisionCode, OvertimeClassificationEntity overtimeClassification);

        /// <summary>
        /// Update the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationEntity">overtime Classification Entity</param> 
        bool UpdateOvertimeClassification(OvertimeClassificationEntity overtimeClassification);

        /// <summary>
        /// Delete the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationEntity">overtime Classification Entity</param> 
        bool DeleteOvertimeClassification(int overtimeClassificationCode);

        /// <summary>
        /// Get list the Overtime Classifications
        /// </summary>        
        /// <returns>The OvertimeClassificationEntity list</returns>
        List<OvertimeClassificationEntity> GetOvertimeClassificationsList(string geographicDivisionCode, int divisionCode);
    }
}
