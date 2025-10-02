using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWorkingTimeRangesBLL<T> where T : WorkingTimeRangesEntity
    {
        /// <summary>
        /// Get List the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangeCode">working Time Range Code</param>
        /// <param name="workingTimeTypeCode">working Time Type Code</param>
        /// <param name="workingStartTime">working Start Time</param>
        /// <param name="workingEndTime">working End Time</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The WorkingTimeRangesEntity List</returns>
        PageHelper<T> GetWorkingTimeRangesList(string geographicDivisionCode, int divisionCode, int workingTimeRangeCode, int workingTimeTypeCode, string workingStartTime, string workingEndTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize);

        /// <summary>
        ///  Get Working Time Ranges By Working Time Range Code
        /// </summary>
        /// <param name="workingTimeRangeCode">Working time Range Code</param>
        /// <returns>WorkingTimeRangesEntity</returns>
        T GetWorkingTimeRangesByWorkingTimeRangeCode(int workingTimeRangeCode);

        /// <summary>
        ///  Get Working Time Ranges By Working Time Range Code
        /// </summary>
        /// <param name="workingTimeRangeCode">Working time Range Code</param>
        /// <returns>WorkingTimeRangesEntity</returns>
        T GetWorkingTimeRangesByHours(WorkingTimeRangesEntity workingTimeRangeEntity);

        /// <summary>
        /// Save the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangesEntity">working Time Ranges Entity</param> 
        bool AddWorkingTimeRanges(string geographicDivisionCode, int divisionCode, WorkingTimeRangesEntity workingTimeRanges);

        /// <summary>
        /// Update the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangesEntity">working Time Ranges Entity</param> 
        bool UpdateWorkingTimeRanges(WorkingTimeRangesEntity workingTimeRanges);

        /// <summary>
        /// Delete the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangesEntity">working Time Ranges Entity</param> 
        bool DeleteWorkingTimeRanges(int workingTimeRangeCode);
    }
}
