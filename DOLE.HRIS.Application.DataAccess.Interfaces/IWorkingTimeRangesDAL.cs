using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWorkingTimeRangesDAL<T> where T : WorkingTimeRangesEntity
    {
        /// <summary>
        /// List the Working Time Ranges
        /// </summary>
        /// <return>Working Time Ranges List</return>
        PageHelper<T> GetWorkingTimeRangesList(string geographicDivisionCode, int divisionCode, int workingTimeRangeCode, int workingTimeTypeCode, string workingStartTime, string workingEndTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// Get DayTypes By Working Time Range Code
        /// </summary>
        /// <param name="workingTimeRangeCode">Working Time Range Code</param>
        T GetWorkingTimeRangesByWorkingTimeRangeCode(int workingTimeRangeCode);

        /// <summary>
        /// Get DayTypes By hours
        /// </summary>
        /// <param name="workingTimeRangeEntity">Working Time Range Entity</param>
        T GetWorkingTimeRangesByHours(WorkingTimeRangesEntity workingTimeRangeEntity);

        /// <summary>
        /// Save the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRanges">WorkingTimeRanges</param>
        bool AddWorkingTimeRanges(string geographicDivisionCode, int divisionCode, WorkingTimeRangesEntity workingTimeRanges);

        /// <summary>
        /// Update the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRanges">WorkingTimeRanges</param>
        bool UpdateWorkingTimeRanges(WorkingTimeRangesEntity workingTimeRanges);

        /// <summary>
        /// Delete the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangeCode">Working Time Range Code</param>
        bool DeleteWorkingTimeRanges(int workingTimeRangeCode);
    }
}
