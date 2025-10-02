using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using DOLE.HRIS.Application.DataAccess.Interfaces;

namespace DOLE.HRIS.Application.DataAccess
{
    public class WorkingDayTypesDAL : IWorkingDayTypesDAL<WorkingDayTypesEntity>
    {
        /// <summary>
        /// Save the WorkingDayTypes Records
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param> 
        public bool AddWorkingDayTypes(WorkingDayTypesEntity workingDayTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingDayTypesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",workingDayTypes.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",workingDayTypes.DivisionCode),
                    new SqlParameter("@WorkingDayTypesName",workingDayTypes.WorkingDayTypesName),
                    new SqlParameter("@CreatedBy",workingDayTypes.CreatedBy)
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
        /// <summary>
        /// Delete a WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypeCode">WorkingDayTypeCode</param>
        public bool DeleteWorkingDayTypes(int workingDayTypeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingDayTypesDelete", new SqlParameter[] {
                    new SqlParameter("@WorkingDayTypeCode",workingDayTypeCode)
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
        /// <summary>
        /// List the WorkingDayTypes
        /// </summary>        
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="workingDayTypeCode">working Day Type Code</param>
        /// <param name="workingDayTypesName">working Day Types Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingDayTypesEntity List</return>
        public PageHelper<WorkingDayTypesEntity> GetWorkingDayTypesList(string geographicDivisionCode, int divisionCode, int workingDayTypeCode, string workingDayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingDayTypesListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode ", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@WorkingDayTypeCode", workingDayTypeCode),
                    new SqlParameter("@WorkingDayTypesName", workingDayTypesName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new WorkingDayTypesEntity
                {
                    WorkingDayTypeCode = r.Field<int>("WorkingDayTypeCode"),
                    WorkingDayTypesName = r.Field<string>("WorkingDayTypesName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<WorkingDayTypesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Update the WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param>
        public bool UpdateWorkingDayTypes(WorkingDayTypesEntity workingDayTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingDayTypesUpdate", new SqlParameter[] {
                    new SqlParameter("@WorkingDayTypeCode",workingDayTypes.WorkingDayTypeCode),
                    new SqlParameter("@WorkingDayTypesName",workingDayTypes.WorkingDayTypesName),
                    new SqlParameter("@LastModifiedUser",workingDayTypes.LastModifiedUser)
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
        /// <summary>
        /// Get WorkingDayTypes By WorkingDayTypeCode
        /// </summary>        
        /// <param name="workingDayTypeCode">WorkingDayTypeCode</param> 
        public WorkingDayTypesEntity WorkingDayTypesByWorkingDayTypeCode(int workingDayTypeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingDayTypesByWorkingDayTypeCode", new SqlParameter[] {
                    new SqlParameter("@WorkingDayTypeCode", workingDayTypeCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new WorkingDayTypesEntity
                {
                    WorkingDayTypeCode = r.Field<int>("WorkingDayTypeCode"),
                    WorkingDayTypesName = r.Field<string>("WorkingDayTypesName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

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
