using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class OvertimeCompaniesDAL : IOvertimeCompaniesDAL<OvertimeCompaniesEntity>
    {
        /// <summary>
        /// Save the Over Time Records
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies</param> 
        public bool AddOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeCompaniesAdd", new SqlParameter[] {
                    new SqlParameter("@OvertimeCompanieName",overtimeCompanies.OvertimeCompanieName),
                    new SqlParameter("@OvertimeCompanieActivated",overtimeCompanies.OvertimeCompanieActivated),
                    new SqlParameter("@CreatedBy",overtimeCompanies.CreatedBy)
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
        /// Delete a OvertimeCompanies
        /// </summary>
        /// <param name="overtimeCompanieCode">Overtime Companie Code</param>
        public bool DeleteOvertimeCompanies(int overtimeCompanieCode)
        {
            bool status = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(HRISConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("OverTime.OvertimeCompaniesDelete", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@OvertimeCompanieCode", SqlDbType.Int).Value = overtimeCompanieCode;
                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            if (reader.GetInt32(0) >= 1)
                            {
                                status = true;
                            }
                        }
                        connection.Close();
                    }
                }
                return status;
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
        /// List the OvertimeCompanies
        /// </summary>        
        /// <param name="divisionCode">division Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="overtimeFromDate">overtime From Date</param>
        /// <param name="overtimeToDate">overtime To Date</param>
        /// <param name="overtimeCreatedFromDate">overtime Created From Date</param>
        /// <param name="overtimeCreatedToDate">overtime Created To Date</param>
        /// <param name="startHourFrom">start Hour From</param>
        /// <param name="startHourTo">start Hour To</param>
        /// <param name="endHourFrom">endHour From</param>
        /// <param name="overTimeStatus">overTime Status</param>
        /// <param name="overtimeNumber">overtime Number</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The OverTimeRecordsEntity List</return>
        public PageHelper<OvertimeCompaniesEntity> GetOvertimeCompaniesList(int overtimeCompanieCode, string overtimeCompanieName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeCompaniesListByFilters", new SqlParameter[] {
                    new SqlParameter("@OvertimeCompanieCode", overtimeCompanieCode),
                    new SqlParameter("@OvertimeCompanieName", overtimeCompanieName),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new OvertimeCompaniesEntity
                {
                    OvertimeCompanieCode = r.Field<int>("OvertimeCompanieCode"),
                    OvertimeCompanieName = r.Field<string>("OvertimeCompanieName"),
                    OvertimeCompanieActivated = r.Field<bool>("OvertimeCompanieActivated"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<OvertimeCompaniesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get OvertimeCompanie By Overtime Companie Code
        /// </summary>        
        /// <param name="overtimeCompanieCode">Overtime Companie Code</param> 
        /// <return>The OverTimeRecordsEntity List</return>
        public OvertimeCompaniesEntity OvertimeCompaniesByOvertimeCompanieCode(int overtimeCompanieCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeCompaniesByOvertimeCompanieCode", new SqlParameter[] {
                    new SqlParameter("@OvertimeCompanieCode", overtimeCompanieCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeCompaniesEntity
                {
                    OvertimeCompanieCode = r.Field<int>("OvertimeCompanieCode"),
                    OvertimeCompanieName = r.Field<string>("OvertimeCompanieName"),
                    OvertimeCompanieActivated = r.Field<bool>("OvertimeCompanieActivated"),
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
        /// Update the Overtime Companies
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies</param>
        public bool UpdateOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OvertimeCompaniesUpdate", new SqlParameter[] {
                    new SqlParameter("@OvertimeCompanieCode",overtimeCompanies.OvertimeCompanieCode),
                    new SqlParameter("@OvertimeCompanieName",overtimeCompanies.OvertimeCompanieName),
                    new SqlParameter("@OvertimeCompanieActivated",overtimeCompanies.OvertimeCompanieActivated),
                    new SqlParameter("@LastModifiedUser",overtimeCompanies.LastModifiedUser)
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
        /// Get OvertimeCompanie list
        /// </summary>        
        /// <return>The OverTimeRecordsEntity List</return>
        public List<OvertimeCompaniesEntity> OvertimeCompanieList()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OvertimeCompanieList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OvertimeCompaniesEntity
                {
                    OvertimeCompanieCode = r.Field<int>("OvertimeCompanieCode"),
                    OvertimeCompanieName = r.Field<string>("OvertimeCompanieName"),
                    OvertimeCompanieActivated = r.Field<bool>("OvertimeCompanieActivated"),
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
