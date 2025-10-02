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
    public class WorkDepartmentsDAL : IWorkDepartmentsDAL<WorkDepartmentsEntity>
    {
        /// <summary>
        /// List the WorkDepartments
        /// </summary>        
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="userCodeID">user Code ID</param>
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="pageSizeParameterModuleCode">page Size Parameter Module Code</param>
        /// <param name="pageSizeParameterName">page Size Parameter Name</param>
        /// <returns>The WorkDepartmentsEntity List</returns>
        public PageHelper<WorkDepartmentsEntity> GetWorkDepartmentsList(string geographicDivisionCode, int divisionCode, int workDepartmentCode, int overtimeCompanieCode, string workDepartmentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkDepartmentsListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@WorkDepartmentCode", workDepartmentCode),
                    new SqlParameter("@OvertimeCompanieCode", overtimeCompanieCode),
                    new SqlParameter("@WorkDepartmentName", workDepartmentCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new WorkDepartmentsEntity
                {
                    WorkDepartmentCode = r.Field<int>("WorkDepartmentCode"),
                    OvertimeCompanieCode = r.Field<int>("OvertimeCompanieCode"),
                    WorkDepartmentName = r.Field<string>("WorkDepartmentName"),
                    WorkDepartmentActived = r.Field<bool>("WorkDepartmentActived"),
                    OvertimeCompanieName = r.Field<string>("OvertimeCompanieName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<WorkDepartmentsEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);       
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
        /// Get Work Departments By WorkDepartmentCode
        /// </summary>        
        /// <param name="workDepartmentCode">Work Department Code</param> 
        public WorkDepartmentsEntity GetWorkDepartmentsByWorkDepartmentCode(int workDepartmentCode)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(HRISConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("OverTime.WorkDepartmentsByWorkDepartmentCode", connection))
                    {
                        SqlParameter[] sqlParameters = new SqlParameter[1];
                        sqlParameters[0] = new SqlParameter("@WorkDepartmentCode", SqlDbType.Int) { Value = workDepartmentCode };
                        DataSet dataSet = Dal.QueryDataSet("OverTime.WorkDepartmentsByWorkDepartmentCode", sqlParameters);
                        WorkDepartmentsEntity workDepartmentsEntity = new WorkDepartmentsEntity();
                        if (dataSet != null && dataSet.Tables.Count > 0)
                        {
                            var result = dataSet.Tables["Table"].AsEnumerable().Select(r => new WorkDepartmentsEntity
                            {
                                WorkDepartmentCode = r.Field<int>("WorkDepartmentCode"),
                                OvertimeCompanieCode = r.Field<int>("OvertimeCompanieCode"),
                                WorkDepartmentName = r.Field<string>("WorkDepartmentName"),
                                WorkDepartmentActived = r.Field<bool>("WorkDepartmentActived"),
                                CreatedBy = r.Field<string>("CreatedBy"),
                                CreatedDate = r.Field<DateTime>("CreatedDate"),
                                LastModifiedUser = r.Field<string>("LastModifiedUser"),
                                LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                            }).ToList();
                            if (result != null && result.Count() > 0)
                            {
                                workDepartmentsEntity = result.FirstOrDefault();
                            }
                        }
                        return workDepartmentsEntity;
                    }
                }
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
        /// Save the Work Departments
        /// </summary>
        /// <param name="workDepartments">WorkDepartments</param>                
        public bool AddWorkDepartments(WorkDepartmentsEntity workDepartmentsEntity)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkDepartmentsAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",workDepartmentsEntity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",workDepartmentsEntity.DivisionCode),
                    new SqlParameter("@OvertimeCompanieName",workDepartmentsEntity.OvertimeCompanieName),
                    new SqlParameter("@WorkDepartmentName",workDepartmentsEntity.WorkDepartmentName),
                    new SqlParameter("@WorkDepartmentActived",workDepartmentsEntity.WorkDepartmentActived),
                    new SqlParameter("@CreatedBy",workDepartmentsEntity.CreatedBy)
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
        /// Update the WorkDepartments
        /// </summary>
        /// <param name="workDepartments">WorkDepartments</param>            
        public bool UpdateWorkDepartments(WorkDepartmentsEntity workDepartments)
        {
            try
            {
                Dal.TransactionScalar("OverTime.WorkDepartmentsUpdate", new SqlParameter[] {
                    new SqlParameter("@WorkDepartmentCode",workDepartments.WorkDepartmentCode),
                    new SqlParameter("@OvertimeCompanieCode",workDepartments.OvertimeCompanieCode),
                    new SqlParameter("@WorkDepartmentName",workDepartments.WorkDepartmentName),
                    new SqlParameter("@WorkDepartmentActived",workDepartments.WorkDepartmentActived),
                    new SqlParameter("@LastModifiedUser",workDepartments.LastModifiedUser)
                });
                return true;
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
        /// Delete a Work Departments
        /// </summary>
        /// <param name="workDepartmentCode">Work Department Code</param>
        public bool DeleteWorkDepartments(int workDepartmentCode)
        {
            try
            {
                Dal.TransactionScalar("OverTime.WorkDepartmentsDelete", new SqlParameter[] {
                    new SqlParameter("@WorkDepartmentCode",workDepartmentCode)
                });
                return true;
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
