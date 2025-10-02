using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business
{
    public class WorkingTimeRangesBLL : IWorkingTimeRangesBLL<WorkingTimeRangesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IWorkingTimeRangesDAL<WorkingTimeRangesEntity> workingTimeRangesDAL;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="workingTimeRangesDAL">Data access object</param>
        public WorkingTimeRangesBLL(IWorkingTimeRangesDAL<WorkingTimeRangesEntity> workingTimeRangesDAL)
        {
            this.workingTimeRangesDAL = workingTimeRangesDAL;
        }

        /// <summary>
        /// Get List the Working Time Ranges
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="workingTimeRangeCode">working Time Range Code</param>
        /// <param name="workingTimeTypeCode">working Time Type Code</param>
        /// <param name="workingStartTime">working Start Time</param>
        /// <param name="workingEndTime">working End Time</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The WorkingTimeRangesEntity List</returns>
        public PageHelper<WorkingTimeRangesEntity> GetWorkingTimeRangesList(string geographicDivisionCode, int divisionCode, int workingTimeRangeCode, int workingTimeTypeCode, string workingStartTime, string workingEndTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<WorkingTimeRangesEntity> response = new PageHelper<WorkingTimeRangesEntity>();
            try
            {
                response = workingTimeRangesDAL.GetWorkingTimeRangesList(
                      geographicDivisionCode 
                    , divisionCode
                    , workingTimeRangeCode
                    , workingTimeTypeCode
                    , workingStartTime
                    , workingEndTime
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSize);
                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        ///  Get Working Time Ranges By Working Time Range Code
        /// </summary>
        /// <param name="workingTimeRangeCode">Working time Range Code</param>
        /// <returns>WorkingTimeRangesEntity</returns>
        public WorkingTimeRangesEntity GetWorkingTimeRangesByWorkingTimeRangeCode(int workingTimeRangeCode)
        {
            try
            {
                return workingTimeRangesDAL.GetWorkingTimeRangesByWorkingTimeRangeCode(workingTimeRangeCode);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        ///  Get Working Time Ranges By Hours
        /// </summary>
        /// <param name="workingTimeRangeEntity">Working time Range Entity</param>
        /// <returns>WorkingTimeRangesEntity</returns>
        public WorkingTimeRangesEntity GetWorkingTimeRangesByHours(WorkingTimeRangesEntity workingTimeRangeEntity)
        {
            try
            {
                return workingTimeRangesDAL.GetWorkingTimeRangesByHours(workingTimeRangeEntity);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Save the Working Time Ranges
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>  
        /// <param name="divisionCode">division Code</param>  
        /// <param name="workingTimeRangesEntity">working Time Ranges Entity</param>  
        public bool AddWorkingTimeRanges(string geographicDivisionCode, int divisionCode, WorkingTimeRangesEntity workingTimeRanges)
        {
            try
            {
                return workingTimeRangesDAL.AddWorkingTimeRanges(geographicDivisionCode, divisionCode, workingTimeRanges);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Update the Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangesEntity">working Time Ranges Entity</param>
        public bool UpdateWorkingTimeRanges(WorkingTimeRangesEntity workingTimeRanges)
        {
            try
            {
                return workingTimeRangesDAL.UpdateWorkingTimeRanges(workingTimeRanges);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Delete a Working Time Ranges
        /// </summary>
        /// <param name="workingTimeRangeCode">Working Time Range Code</param>
        public bool DeleteWorkingTimeRanges(int workingTimeRangeCode)
        {
            try
            {
                return workingTimeRangesDAL.DeleteWorkingTimeRanges(workingTimeRangeCode);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
    }
}