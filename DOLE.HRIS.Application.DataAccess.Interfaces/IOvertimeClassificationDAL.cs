using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface IOvertimeClassificationDAL<T> where T : OvertimeClassificationEntity
    {
        /// <summary>
        /// List the Overtime Classification
        /// </summary>        
        /// <returns>The Overtime Classification List</returns>
        PageHelper<T> GetOvertimeClassificationList(int daytype, string sortExpression, string sortDirection, int pageNumber, int? pageSize, OvertimeClassificationEntity overtimeClassification);
        /// <summary>
        /// Get Overtime Classification By Code
        /// </summary>        
        /// <param name="overtimeClassificationCode">Overtime Classification Code</param> 
        /// /// <returns>The Overtime Classification</returns>
        T GetOvertimeClassificationByCode(int overtimeClassificationCode);

        /// <summary>
        /// Save the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassification">OvertimeClassification</param>   
        bool AddOvertimeClassification(string geographicDivisionCode, int divisionCode, OvertimeClassificationEntity overtimeClassification);

        /// <summary>
        /// Update the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassification">OvertimeClassification</param>  
        bool UpdateOvertimeClassification(OvertimeClassificationEntity overtimeClassification);

        /// <summary>
        /// Delete the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassification">OvertimeClassification</param>  
        bool DeleteOvertimeClassification(int overtimeClassificationCode);

        /// <summary>
        /// List the Overtime Classification
        /// </summary>    
        /// <param name="geographicDivisionCode">geographic Division Code</param> 
        /// <param name="geographicDivisionCode">division Code</param>
        /// <returns>The Overtime Classification List</returns>
        List<OvertimeClassificationEntity> GetOvertimeClassificationsList(string geographicDivisionCode, int DivisionCode);
    }
}
