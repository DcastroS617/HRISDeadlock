using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using DOLE.HRIS.Application.DataAccess.Interfaces;

namespace DOLE.HRIS.Application.DataAccess
{
    public class WorkingTimeRangesDAL : IWorkingTimeRangesDAL<WorkingTimeRangesEntity>
    {
        /// <summary>
        /// List the WorkingTimeRanges
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
            try
            {
                List<WorkingTimeRangesEntity> workingTimeRangesEntities = new List<WorkingTimeRangesEntity>();

                var ds = Dal.QueryDataSet("OverTime.WorkingTimeRangesListByFilters", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@WorkingTimeRangeCode", workingTimeRangeCode),
                    new SqlParameter("@WorkingTimeTypeCode", workingTimeTypeCode),
                    new SqlParameter("@WorkingStartTime", workingStartTime),
                    new SqlParameter("@WorkingEndTime", workingEndTime),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new WorkingTimeRangesEntity
                {
                    WorkingTimeRangeCode = r.Field<int>("WorkingTimeRangeCode"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    WorkingTimeTypeName = r.Field<string>("WorkingTimeTypeName"),
                    WorkingStartTime = r.Field<string>("WorkingStartTime"),
                    WorkingEndTime = r.Field<string>("WorkingEndTime"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<WorkingTimeRangesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        ///  Get WorkingTimeRanges By WorkingTimeRangeCode
        /// </summary>
        /// <param name="workingTimeRangeCode"> WorkingtimeRangeCode</param>
        public WorkingTimeRangesEntity GetWorkingTimeRangesByWorkingTimeRangeCode(int workingTimeRangeCode)
        {
            try
            {
                WorkingTimeRangesEntity workingTimeRangesEntities = new WorkingTimeRangesEntity();
                var dataSet = Dal.QueryDataSet("OverTime.WorkingTimeRangesByWorkingTimeRangeCode", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeRangeCode",workingTimeRangeCode)
                });

                workingTimeRangesEntities = dataSet.Tables[0].AsEnumerable().Select(r => new WorkingTimeRangesEntity
                {
                    WorkingTimeRangeCode = r.Field<int>("WorkingTimeRangeCode"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    WorkingStartTime = r.Field<string>("WorkingStartTime"),
                    WorkingEndTime = r.Field<string>("WorkingEndTime"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return workingTimeRangesEntities;
            }
            catch(SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        ///  Get WorkingTimeRanges By hours
        /// </summary>
        /// <param name="workingTimeRangeEntity"> workingTimeRangeEntity</param>
        public WorkingTimeRangesEntity GetWorkingTimeRangesByHours(WorkingTimeRangesEntity workingTimeRangeEntity)
        {
            try
            {
                WorkingTimeRangesEntity workingTimeRangesEntities = new WorkingTimeRangesEntity();
                var dataSet = Dal.QueryDataSet("OverTime.WorkingTimeRangeByHours", new SqlParameter[] {
                    new SqlParameter("@StarHour",workingTimeRangeEntity.WorkingStartTime),
                    new SqlParameter("@EndHour",workingTimeRangeEntity.WorkingEndTime)
                });

                workingTimeRangesEntities = dataSet.Tables[0].AsEnumerable().Select(r => new WorkingTimeRangesEntity
                {
                    WorkingTimeRangeCode = r.Field<int>("WorkingTimeRangeCode"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    WorkingStartTime = r.Field<string>("WorkingStartTime"),
                    WorkingEndTime = r.Field<string>("WorkingEndTime")
                }).FirstOrDefault();

                return workingTimeRangesEntities;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// Save the WorkingTimeRanges
        /// </summary>
        /// <param name="workingTimeRanges">WorkingTimeRanges</param>        
        public bool AddWorkingTimeRanges(string geographicDivisionCode, int divisionCode, WorkingTimeRangesEntity workingTimeRanges)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeRangesAdd", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeRanges.WorkingTimeTypeCode),
                    new SqlParameter("@WorkingStartTime",workingTimeRanges.WorkingStartTime),
                    new SqlParameter("@WorkingEndTime",workingTimeRanges.WorkingEndTime),
                    new SqlParameter("@CreatedBy",workingTimeRanges.CreatedBy)
                });
                return result >= 1;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Update the WorkingTimeRanges
        /// </summary>
        /// <param name="workingTimeRanges">WorkingTimeRanges</param>
        public bool UpdateWorkingTimeRanges(WorkingTimeRangesEntity workingTimeRanges)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeRangesUpdate", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeRangeCode",workingTimeRanges.WorkingTimeRangeCode),
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeRanges.WorkingTimeTypeCode),
                    new SqlParameter("@WorkingStartTime",workingTimeRanges.WorkingStartTime),
                    new SqlParameter("@WorkingEndTime",workingTimeRanges.WorkingEndTime),
                    new SqlParameter("@LastModifiedUser",workingTimeRanges.LastModifiedUser)
                });
                return result >= 1;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Delete a WorkingTimeRanges
        /// </summary>
        /// <param name="workingTimeRangeCode">Working Time Range Code</param
        public bool DeleteWorkingTimeRanges(int workingTimeRangeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeRangesDelete", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeRangeCode",workingTimeRangeCode)
                });
                return result == 0;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }   
    }
}