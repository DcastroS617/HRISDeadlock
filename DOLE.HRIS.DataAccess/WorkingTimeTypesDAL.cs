using DOLE.HRIS.Application.DataAccess.Interfaces;
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

namespace DOLE.HRIS.Application.DataAccess
{
    public class WorkingTimeTypesDAL : IWorkingTimeTypesDAL<WorkingTimeTypesEntity>
    {
        /// <summary>
        /// List the WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        /// <param name="workingTimeTypeName">working Time Type Name</param>
        /// <param name="totalWorkingTime">total Working Time</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeTypesEntity List</return>
        public PageHelper<WorkingTimeTypesEntity> GetWorkingTimeTypesList(int divisionCode, int workingTimeTypeCode, string workingTimeTypeName, int totalWorkingTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingTimeTypesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@WorkingTimeTypeCode", workingTimeTypeCode),
                    new SqlParameter("@WorkingTimeTypeName", workingTimeTypeName),
                    new SqlParameter("@TotalWorkingTime", totalWorkingTime),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSize)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new WorkingTimeTypesEntity
                {
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    WorkingTimeTypeName = r.Field<string>("WorkingTimeTypeName"),
                    WorkingTimeTypeDescription = r.Field<string>("WorkingTimeTypeDescription"),
                    TotalWorkingTime = r.Field<int>("TotalWorkingTime"),
                    MaxWorkingTime = r.Field<int>("MaxWorkingTime"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<WorkingTimeTypesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        ///  Get WorkingTimeTypes By WorkingTimeTypeCode
        /// </summary>
        /// <param name="workingTimeTypeCode">WorkingTimeTypeCode</param>
        public WorkingTimeTypesEntity GetWorkingTimeTypesByWorkingTimeTypeCode(int workingTimeTypeCode)
        {
            try
            {
                WorkingTimeTypesEntity workingTimeTypesEntities = new WorkingTimeTypesEntity();
                var dataSet = Dal.QueryDataSet("OverTime.WorkingTimeTypesByWorkingTimeTypeCode", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeTypeCode)
                });

                workingTimeTypesEntities = dataSet.Tables[0].AsEnumerable().Select(r => new WorkingTimeTypesEntity
                {
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    WorkingTimeTypeName = r.Field<string>("WorkingTimeTypeName"),
                    WorkingTimeTypeDescription = r.Field<string>("WorkingTimeTypeDescription"),
                    TotalWorkingTime = r.Field<int>("TotalWorkingTime"),
                    MaxWorkingTime = r.Field<int>("MaxWorkingTime"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate")
                }).FirstOrDefault();

                return workingTimeTypesEntities;
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
        ///  Save the WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypes">WorkingTimeTypes</param>
        public bool AddWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeTypesAdd", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",workingTimeTypes.DivisionCode),
                    new SqlParameter("@WorkingTimeTypeName",workingTimeTypes.WorkingTimeTypeName),
                    new SqlParameter("@WorkingTimeTypeDescription",workingTimeTypes.WorkingTimeTypeDescription),
                    new SqlParameter("@TotalWorkingTime",workingTimeTypes.TotalWorkingTime),
                    new SqlParameter("@MaxWorkingTime",workingTimeTypes.MaxWorkingTime),
                    new SqlParameter("@CreatedBy",workingTimeTypes.CreatedBy)
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
        /// Update the WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypes">WorkingTimeTypes</param>
        public bool UpdateWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeTypesUpdate", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeTypes.WorkingTimeTypeCode),
                    new SqlParameter("@DivisionCode",workingTimeTypes.DivisionCode),
                    new SqlParameter("@WorkingTimeTypeName",workingTimeTypes.WorkingTimeTypeName),
                    new SqlParameter("@WorkingTimeTypeDescription",workingTimeTypes.WorkingTimeTypeDescription),
                    new SqlParameter("@TotalWorkingTime",workingTimeTypes.TotalWorkingTime),
                    new SqlParameter("@MaxWorkingTime",workingTimeTypes.MaxWorkingTime),
                    new SqlParameter("@LastModifiedUser",workingTimeTypes.LastModifiedUser)
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
        /// Delete a WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Code</param>
        public bool DeleteWorkingTimeTypes(int workingTimeTypeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeTypesDelete", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeTypeCode)
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
        /// List a WorkingTimeTypes
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Code</param>
        public List<WorkingTimeTypesEntity> GetWorkingTimeTypesListForDropdown()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingTimeTypesListForDropdown");

                var result = ds.Tables[0].AsEnumerable().Select(r => new WorkingTimeTypesEntity
                {
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    WorkingTimeTypeName = r.Field<string>("WorkingTimeTypeName"),
                    WorkingTimeTypeDescription = r.Field<string>("WorkingTimeTypeDescription"),
                    TotalWorkingTime = r.Field<int>("TotalWorkingTime"),
                    MaxWorkingTime = r.Field<int>("MaxWorkingTime")
                }).ToList();

                return result;
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
    }
}
