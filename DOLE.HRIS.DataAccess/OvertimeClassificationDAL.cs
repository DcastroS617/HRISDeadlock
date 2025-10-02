using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class OvertimeClassificationDAL : IOvertimeClassificationDAL<OvertimeClassificationEntity>
    {
        /// <summary>
        /// List the Overtime Classification
        /// </summary>        
        /// <returns>The Overtime Classification List</returns>
        public PageHelper<OvertimeClassificationEntity> GetOvertimeClassificationList(int daytype, string sortExpression, string sortDirection, int pageNumber, int? pageSize, OvertimeClassificationEntity overtimeClassification)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeClassificationListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",overtimeClassification.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",overtimeClassification.DivisionCode),
                    new SqlParameter("@OvertimeClassificationCode", overtimeClassification.OvertimeClassificationCode),
                    new SqlParameter("@OvertimeTypeCode", overtimeClassification.OvertimeTypeCode),
                    new SqlParameter("@OvertimeClassificationName", overtimeClassification.OvertimeClassificationName),
                    new SqlParameter("@DaytypeCode", daytype),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new OvertimeClassificationEntity
                {
                    OvertimeClassificationCode = r.Field<int>("OvertimeClassificationCode"),
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    OvertimeClassificationName = r.Field<string>("OvertimeClassificationName"),
                    OvertimeTypeName = r.Field<string>("OvertimeTypeName"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    WorkingTimeTypeName = r.Field<string>("WorkingTimeTypeName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    DayTypesName = r.Field<string>("DayTypesName"),
                }).ToList();

                return new PageHelper<OvertimeClassificationEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Overtime Classification By Code
        /// </summary>        
        /// <param name="overtimeClassificationCode">Overtime Classification Code</param> 
        /// /// <returns>The Overtime Classification</returns>
        public OvertimeClassificationEntity GetOvertimeClassificationByCode(int overtimeClassificationCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeClassificationByCode", new SqlParameter[] {
                    new SqlParameter("@OvertimeClassificationCode", overtimeClassificationCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeClassificationEntity
                {
                    OvertimeClassificationCode = r.Field<int>("OvertimeClassificationCode"),
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    OvertimeClassificationName = r.Field<string>("OvertimeClassificationName"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    IsExtra = r.Field<bool>("IsExtra"),
                    DayTypeCode= r.Field<int>("DayTypeCode"),
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

        /// <summary>
        /// Save the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassification">OvertimeClassification</param>                
        public bool AddOvertimeClassification(string geographicDivisionCode, int divisionCode, OvertimeClassificationEntity overtimeClassification)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeClassificationAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@OvertimeTypeCode",overtimeClassification.OvertimeTypeCode),
                    new SqlParameter("@OvertimeClassificationName",overtimeClassification.OvertimeClassificationName),
                    new SqlParameter("@WorkingTimeTypeCode",overtimeClassification.WorkingTimeTypeCode),
                    new SqlParameter("@DayTypeCode",overtimeClassification.DayTypeCode),
                    new SqlParameter("@IsExtra",overtimeClassification.IsExtra),
                    new SqlParameter("@CreatedBy",overtimeClassification.CreatedBy)
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
        /// Update the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassification">OvertimeClassification</param>            
        public bool UpdateOvertimeClassification(OvertimeClassificationEntity overtimeClassification)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeClassificationUpdate", new SqlParameter[] {
                    new SqlParameter("@OvertimeClassificationCode",overtimeClassification.OvertimeClassificationCode),
                    new SqlParameter("@OvertimeTypeCode",overtimeClassification.OvertimeTypeCode),
                    new SqlParameter("@OvertimeClassificationName",overtimeClassification.OvertimeClassificationName),
                    new SqlParameter("@WorkingTimeTypeCode",overtimeClassification.WorkingTimeTypeCode),
                    new SqlParameter("@DayTypeCode",overtimeClassification.DayTypeCode),
                    new SqlParameter("@IsExtra",overtimeClassification.IsExtra),
                    new SqlParameter("@LastModifiedUser",overtimeClassification.LastModifiedUser)
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
        /// Delete a Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationCode">OvertimeClassificationCode</param>
        public bool DeleteOvertimeClassification(int overtimeClassificationCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeClassificationDelete", new SqlParameter[] {
                    new SqlParameter("@OvertimeClassificationCode",overtimeClassificationCode),
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
        /// List the Overtime Classification
        /// </summary>  
        /// <param name="geographicDivisionCode">geographic Division Code</param> 
        /// <param name="geographicDivisionCode">division Code</param> 
        /// <returns>The Overtime Classification List</returns>
        public List<OvertimeClassificationEntity> GetOvertimeClassificationsList(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeClassificationsList", new SqlParameter[] 
                {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeClassificationEntity
                {
                    OvertimeClassificationCode = r.Field<int>("OvertimeClassificationCode"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    OvertimeClassificationName = r.Field<string>("OvertimeClassificationName"),
                    DayTypeCode = r.Field<int>("DayTypeCode"),
                    IsExtra = r.Field<bool>("IsExtra"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
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
