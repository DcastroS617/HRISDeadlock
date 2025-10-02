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
    public class OvertimeTypesDAL : IOvertimeTypesDAL<OvertimeTypesEntity>
    {
        /// <summary>
        /// List the Overtime Types
        /// </summary>        
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="overtimeTypeCode">overtime Type Code</param>
        /// <param name="overtimeTypeName">overtime Type Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The OvertimeTypesEntity List</return>
        public PageHelper<OvertimeTypesEntity> GetOvertimeTypesList(string geographicDivisionCode, int divisionCode, int overtimeTypeCode, string overtimeTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeTypesListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@OvertimeTypeCode", overtimeTypeCode),
                    new SqlParameter("@OvertimeTypeName", overtimeTypeName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new OvertimeTypesEntity
                {
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    OvertimeTypeName = r.Field<string>("OvertimeTypeName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<OvertimeTypesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Overtime Types By Overtime Type Code
        /// </summary>        
        /// <param name="overtimeTypeCode">Overtime Type Code</param> 
        public OvertimeTypesEntity GetOvertimeTypesByOvertimeTypeCode(int overtimeTypeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeTypesByOvertimeTypeCode", new SqlParameter[] {
                    new SqlParameter("@OvertimeTypeCode", overtimeTypeCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeTypesEntity
                {
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    OvertimeTypeName = r.Field<string>("OvertimeTypeName"),
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

        /// <summary>
        /// Save the Overtime Types
        /// </summary>
        /// <param name="overtimeTypes">Overtime Types</param>                
        public bool AddOvertimeTypes(OvertimeTypesEntity overtimeTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeTypesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",overtimeTypes.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",overtimeTypes.DivisionCode),
                    new SqlParameter("@OvertimeTypeName",overtimeTypes.OvertimeTypeName),
                    new SqlParameter("@CreatedBy",overtimeTypes.CreatedBy)
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
        /// Update the Overtime Types
        /// </summary>
        /// <param name="overtimeTypes">Overtime Types</param>            
        public bool UpdateOvertimeTypes(OvertimeTypesEntity overtimeTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeTypesUpdate", new SqlParameter[] {
                    new SqlParameter("@OvertimeTypeCode",overtimeTypes.OvertimeTypeCode),
                    new SqlParameter("@DivisionCode",overtimeTypes.DivisionCode),
                    new SqlParameter("@OvertimeTypeName",overtimeTypes.OvertimeTypeName),
                    new SqlParameter("@LastModifiedUser",overtimeTypes.LastModifiedUser)
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
        /// Delete Overtime Types
        /// </summary>
        /// <param name="overtimeTypeCode">Overtime Type Code</param>
        public bool DeleteOvertimeTypes(int overtimeTypeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeTypesDelete", new SqlParameter[] {
                    new SqlParameter("@OvertimeTypeCode",overtimeTypeCode)
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

        public List<OvertimeTypesEntity> GetOvertimeTypesListForDropdown()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeTypesListForDropdown",null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeTypesEntity
                {
                    OvertimeTypeCode = r.Field<int>("OvertimeTypeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    OvertimeTypeName = r.Field<string>("OvertimeTypeName"),
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
