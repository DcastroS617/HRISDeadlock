using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class OvertimeApprovalTypesDAL : IOvertimeApprovalTypesDAL<OvertimeApprovalTypesEntity>
    {

        /// <summary>
        /// Save the OvertimeApprovalTypes Records
        /// </summary>
        /// <param name="overtimeApprovalTypes">OvertimeApprovalTypes</param> 
        public bool AddOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeApprovalTypesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",overtimeApprovalTypes.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",overtimeApprovalTypes.DivisionCode),
                    new SqlParameter("@OvertimeApprovalTypeName",overtimeApprovalTypes.OvertimeApprovalTypeName),
                    new SqlParameter("@CreatedBy",overtimeApprovalTypes.CreatedBy)
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
        /// Delete a OvertimeApprovalTypes
        /// </summary>
        /// <param name="overtimeApprovalTypeCode">OvertimeApprovalTypesCode</param>
        public bool DeleteOvertimeApprovalTypes(int overtimeApprovalTypeCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeApprovalTypesDelete", new SqlParameter[] {
                    new SqlParameter("@OvertimeApprovalTypeCode",overtimeApprovalTypeCode)
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
        /// List the OvertimeApprovalTypes
        /// </summary>        
        ///  /// Get list the Overtime Approval Types     
        /// <param name="geographicDivisionCode">geographic Division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="overtimeApprovalTypeCode">overtime Approval Type Code</param>
        /// <param name="overtimeApprovalTypeName">overtime Approval Type Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The OvertimeApprovalTypes List</returns>
        public PageHelper<OvertimeApprovalTypesEntity> GetOvertimeApprovalTypesList(string geographicDivisionCode, int divisionCode, int overtimeApprovalTypeCode, string overtimeApprovalTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeApprovalTypesListByFilters", new SqlParameter[] {
                        new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                        new SqlParameter("@DivisionCode", divisionCode),
                        new SqlParameter("@OvertimeApprovalTypeCode", (object)overtimeApprovalTypeCode == null || overtimeApprovalTypeCode == 0 ? (object)DBNull.Value : overtimeApprovalTypeCode),
                        new SqlParameter("@OvertimeApprovalTypeName", string.IsNullOrEmpty(overtimeApprovalTypeName) ? (object)DBNull.Value : overtimeApprovalTypeName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new OvertimeApprovalTypesEntity
                {
                    OvertimeApprovalTypeCode = r.Field<int>("OvertimeApprovalTypeCode"),
                    OvertimeApprovalTypeName = r.Field<string>("OvertimeApprovalTypeName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();
                return new PageHelper<OvertimeApprovalTypesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get OvertimeApprovalTypes By OvertimeApprovalTypeCode
        /// </summary>        
        /// <param name="overtimeApprovalTypeCode">OvertimeApprovalTypeCode</param> 
        public OvertimeApprovalTypesEntity OvertimeApprovalTypesByOvertimeApprovalTypeCode(int overtimeApprovalTypeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeApprovalTypesByOvertimeApprovalTypeCode", new SqlParameter[] {
                    new SqlParameter("@OvertimeApprovalTypeCode", overtimeApprovalTypeCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeApprovalTypesEntity
                {
                    OvertimeApprovalTypeCode = r.Field<int>("OvertimeApprovalTypeCode"),
                    OvertimeApprovalTypeName = r.Field<string>("OvertimeApprovalTypeName"),
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
        /// Update the OvertimeApprovalTypes
        /// </summary>
        /// <param name="overtimeApprovalTypes">OvertimeApprovalTypes</param>
        public bool UpdateOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeApprovalTypesUpdate", new SqlParameter[] {
                    new SqlParameter("@OvertimeApprovalTypeCode",overtimeApprovalTypes.OvertimeApprovalTypeCode),
                    new SqlParameter("@OvertimeApprovalTypeName",overtimeApprovalTypes.OvertimeApprovalTypeName),
                    new SqlParameter("@LastModifiedUser",overtimeApprovalTypes.LastModifiedUser)
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
