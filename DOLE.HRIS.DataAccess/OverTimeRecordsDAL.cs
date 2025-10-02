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
    public class OverTimeRecordsDAL : IOverTimeRecordsDAL<OverTimeRecordsEntity>
    {
        public static readonly DateTime MinDate = new DateTime(1753, 1, 1);
        /// <summary>
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
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
        public PageHelper<OverTimeRecordsEntity> GetOverTimeRecordList(string geographicDivisionCode, int divisionCode, string userEmployeeCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string assignTo , string deparmentCode)
        {
            try
            {
                DateTime? initialDate = overtimeFromDate >= MinDate ? overtimeFromDate : (DateTime?)null;
                DateTime? finalDate = overtimeToDate >= MinDate ? overtimeToDate : (DateTime?)null;

                var ds = Dal.QueryDataSet("OverTime.OverTimeRecordListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@UserEmployeeCode", userEmployeeCode),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@DateTypeFilter", dateType),
                    new SqlParameter("@OvertimeFromDate", initialDate.HasValue ? (object)initialDate.Value : DBNull.Value),
                    new SqlParameter("@OvertimeToDate", finalDate.HasValue ? (object)finalDate.Value : DBNull.Value),
                    new SqlParameter("@OvertimeCreatedFromDate", initialDate.HasValue ? (object)initialDate.Value : DBNull.Value),
                    new SqlParameter("@OvertimeCreatedToDate", finalDate.HasValue ? (object)finalDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59) : DBNull.Value),
                    new SqlParameter("@StartHourFrom", startHourFrom),
                    new SqlParameter("@StartHourTo", startHourTo),
                    new SqlParameter("@EndHourFrom", endHourFrom),
                    new SqlParameter("@EndHourTo", endHourTo),
                    new SqlParameter("@OverTimeStatus", overTimeStatus),
                    new SqlParameter("@OvertimeNumber", overtimeNumber),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSize),
                    new SqlParameter("@DepartmentCode", deparmentCode)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    Employee = r.Field<string>("Employee"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatus = r.Field<string>("OverTimeStatus"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    Company = r.Field<string>("Company"),
                    AssignToUser = r.Field<string>("AssignToUser"),
                    WorkingTimeTypeDescription = r.Field<string>("WorkingTimeTypeDescription")
                }).ToList();

                return new PageHelper<OverTimeRecordsEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, "Sucedió un error al consultar el registro de horas extras"), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException("Sucedió un error al consultar el registro de horas extras", ex);
                }
            }
        }

        /// <summary>
        /// Get List the OverTime Record
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
        public PageHelper<OverTimeRecordsEntity> GetOverTimeRecordPayrollList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string sortExpression, string sortDirection, int pageNumber, int? pageSize, string deparmentCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimePayrollAnalystApprovalListByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@DateTypeFilter", dateType),
                    new SqlParameter("@OvertimeFromDate", overtimeFromDate),
                    new SqlParameter("@OvertimeToDate", overtimeToDate),
                    new SqlParameter("@OvertimeCreatedFromDate", overtimeFromDate),
                    new SqlParameter("@OvertimeCreatedToDate", overtimeToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59)),
                    new SqlParameter("@StartHourFrom", startHourFrom),
                    new SqlParameter("@StartHourTo", startHourTo),
                    new SqlParameter("@EndHourFrom", endHourFrom),
                    new SqlParameter("@EndHourTo", endHourTo),
                    new SqlParameter("@OverTimeStatus", overTimeStatus),
                    new SqlParameter("@OvertimeNumber", overtimeNumber),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSize),
                    new SqlParameter("@DepartmentCode", deparmentCode)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    Employee = r.Field<string>("Employee"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatus = r.Field<string>("OverTimeStatus"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    Company = r.Field<string>("Company"),
                    AssignToUser = r.Field<string>("AssignToUser"),
                    OvertimeClassificationName = r.Field<string>("OvertimeTypeName"),
                    WorkingTimeTypeDescription = r.Field<string>("WorkingTimeTypeDescription")
                }).ToList();

                return new PageHelper<OverTimeRecordsEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Over Time Record By OverTimeNumber
        /// </summary>        
        /// <param name="OverTimeNumber">Over Time Number</param> 
        public OverTimeRecordsEntity GetOverTimeRecordByOverTimeNumber(int overTimeNumber)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeRecordByOverTimeNumber", new SqlParameter[] {
                    new SqlParameter("@OverTimeNumber", overTimeNumber),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    CompanieCode = r.Field<int>("CompanieCode"),
                    ApprovalRemark = r.Field<string>("ApprovalRemark")
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
        /// Get Over Time Record By Filters
        /// </summary>        
        /// <param name="overTimeRecordsEntity">Over Time entity</param> 
        public List<OverTimeRecordsEntity> GetOvertimeRecordsByFilters(OverTimeRecordsEntity overTimeRecordsEntity)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeRecordByFilters", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", overTimeRecordsEntity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", overTimeRecordsEntity.DivisionCode),
                    new SqlParameter("@EmployeeCode", overTimeRecordsEntity.EmployeeCode),
                    new SqlParameter("@OvertimeDate", overTimeRecordsEntity.OvertimeDate)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    CompanieCode = r.Field<int>("CompanieCode"),
                    ApprovalRemark = r.Field<string>("ApprovalRemark")
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

        /// <summary>
        /// Save the Over Time Records
        /// </summary>
        /// <param name="overTimeRecords">Over Time Records</param>                
        public bool AddOverTimeRecord(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeRecordAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",overTimeRecords.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",overTimeRecords.DivisionCode),
                    new SqlParameter("@EmployeeCode",overTimeRecords.EmployeeCode),
                    new SqlParameter("@StartHour",overTimeRecords.StartHour),
                    new SqlParameter("@EndHour",overTimeRecords.EndHour),
                    new SqlParameter("@OverTimeStatusCode",overTimeRecords.OverTimeStatusCode),
                    new SqlParameter("@JustificationForExtraTime",overTimeRecords.JustificationForExtraTime),
                    new SqlParameter("@IsExtraHour",overTimeRecords.IsExtraHour),
                    new SqlParameter("@CreatedBy",overTimeRecords.CreatedBy),
                    new SqlParameter("@OvertimeDate",overTimeRecords.OvertimeDate),
                    new SqlParameter("@ApprovalRemark",overTimeRecords.ApprovalRemark),
                    new SqlParameter("@CompanieCode",overTimeRecords.CompanieCode),
                    new SqlParameter("@DepartmentCode",overTimeRecords.DepartmentCode),
                    new SqlParameter("@WorkingTimeRangeCode",overTimeRecords.WorkingTimeRangeCode),
                    new SqlParameter("@OvertimeClassificationCode",overTimeRecords.OvertimeClassificationCode)
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
        /// Update the Over Time Records
        /// </summary>
        /// <param name="overTimeRecords">Over Time Records</param>                
        public bool UpdateOverTimeRecord(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeRecordUpdate", new SqlParameter[] {
                    new SqlParameter("@OverTimeNumber",overTimeRecords.OverTimeNumber),
                    new SqlParameter("@StartHour",overTimeRecords.StartHour),
                    new SqlParameter("@EndHour",overTimeRecords.EndHour),
                    new SqlParameter("@OverTimeStatusCode",overTimeRecords.OverTimeStatusCode),
                    new SqlParameter("@JustificationForExtraTime",overTimeRecords.JustificationForExtraTime),
                    new SqlParameter("@IsExtraHour",overTimeRecords.IsExtraHour),
                    new SqlParameter("@OvertimeDate",overTimeRecords.OvertimeDate),
                    new SqlParameter("@LastModifiedUser",overTimeRecords.LastModifiedUser),
                    new SqlParameter("@CompanieCode",overTimeRecords.CompanieCode),
                    new SqlParameter("@WorkingTimeRangeCode",overTimeRecords.WorkingTimeRangeCode),
                    new SqlParameter("@OvertimeClassificationCode",overTimeRecords.OvertimeClassificationCode)
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
        /// Delete an OverTimeRecord
        /// </summary>
        /// <param name="overTimeNumber">Over Time Number</param>
        public bool DeleteOverTimeRecord(int overTimeNumber)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeRecordDelete", new SqlParameter[] {
                    new SqlParameter("@OverTimeNumber",overTimeNumber)
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
        /// Update the Over Time Records Status
        /// </summary>
        /// <param name="overTimeRecords">Over Time Records</param>                
        public bool UpdateOverTimeRecordStatus(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeRecordStatusUpdate", new SqlParameter[] {
                    new SqlParameter("@OverTimeNumber",overTimeRecords.OverTimeNumber),
                    new SqlParameter("@OverTimeStatusCode",overTimeRecords.OverTimeStatusCode),
                    new SqlParameter("@ApprovalRemark",overTimeRecords.ApprovalRemark),
                    new SqlParameter("@LastModifiedUser",overTimeRecords.LastModifiedUser)
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
        /// Insert log to OvertimeApprovalLog everytime when status for overtime get updated.
        /// </summary>
        /// <param name="overTimeRecords"></param>
        /// <returns></returns>
        /// <exception cref="DataAccessException"></exception>
        public bool AddOverTimeRecordApprovalLog(OverTimeRecordsEntity overTimeRecords)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.OverTimeApprovalLogAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",overTimeRecords.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",overTimeRecords.DivisionCode),
                    new SqlParameter("@OverTimeNumber",overTimeRecords.OverTimeNumber),
                    new SqlParameter("@ApprovalStatusCode",overTimeRecords.OverTimeStatusCode),
                    new SqlParameter("@AssignTo",overTimeRecords.AssignTo),
                    new SqlParameter("@CreatedBy",overTimeRecords.CreatedBy),
                    new SqlParameter("@ApprovalRemark",overTimeRecords.ApprovalRemark)
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
        /// Get list the OverTime Payroll Analyst Approval
        /// </summary>        
        /// <param name="divisionCode">division Code</param> 
        /// <return>The OverTimeRecordsEntity List</return>
        public List<OverTimeRecordsEntity> GetOverTimePayrollAnalystApprovalList(string geographicDivisionCode, int divisionCode, string employeeCode, int dateType, DateTime? overtimeFromDate, DateTime? overtimeToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber, string deparmentCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimePayrollAnalystApprovalList", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@DateTypeFilter", dateType),
                    new SqlParameter("@OvertimeFromDate", overtimeFromDate),
                    new SqlParameter("@OvertimeToDate", overtimeToDate),
                    new SqlParameter("@OvertimeCreatedFromDate", overtimeFromDate),
                    new SqlParameter("@OvertimeCreatedToDate", overtimeToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59)),
                    new SqlParameter("@StartHourFrom", startHourFrom),
                    new SqlParameter("@StartHourTo", startHourTo),
                    new SqlParameter("@EndHourFrom", endHourFrom),
                    new SqlParameter("@EndHourTo", endHourTo),
                    new SqlParameter("@OverTimeStatus", overTimeStatus),
                    new SqlParameter("@OvertimeNumber", overtimeNumber),
                    new SqlParameter("@DepartmentCode", deparmentCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    Employee = r.Field<string>("Employee"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatus = r.Field<string>("OverTimeStatus"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    Company = r.Field<string>("Company"),
                    AssignToUser = r.Field<string>("AssignToUser"),
                    OvertimeClassificationName = r.Field<string>("OvertimeTypeName")
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
        /// <summary>
        /// Get Working Time Hours By Department Employees
        /// </summary>        
        /// <param name="sqlQuery">sql Query</param> 
        /// <return>WorkingTimeHoursByDepartmentEmployeesEntity</return>
        public WorkingTimeHoursByDepartmentEmployeesEntity GetWorkingTimeHoursByDepartmentEmployees(OverTimeEmployeeView overTimeEmployeeView, string sDate)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingTimeHoursByDepartmentEmployeesByEmployee", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", overTimeEmployeeView.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", overTimeEmployeeView.DivisionCode),
                    new SqlParameter("@DepartmentCode", overTimeEmployeeView.DepartmentCode),
                    new SqlParameter("@EmployeeCode", overTimeEmployeeView.EmployeeCode),
                    new SqlParameter("@StarDate", sDate)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new WorkingTimeHoursByDepartmentEmployeesEntity
                {
                    WorkingTimeHoursByDepartmentEmployeesID = r.Field<int>("WorkingTimeHoursByDepartmentEmployeesID"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    StartDate = r.Field<DateTime>("StartDate"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    WorkingTimeTypeCode = r.Field<int>("WorkingTimeTypeCode"),
                    RestDay = r.Field<string>("RestDay"),
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
        /// Get OverTime Employee
        /// </summary>        
        /// <param name="activeDirectoryUserAccount">active Directory User Account</param> 
        /// <return>OverTimeEmployeeView</return>
        public OverTimeEmployeeView GetOverTimeEmployee(string activeDirectoryUserAccount)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.EmployeeByActiveDirectoryUser", new SqlParameter[] {
                    new SqlParameter("@ActiveDirectoryUserAccount", activeDirectoryUserAccount)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeEmployeeView
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CurrentMaritalStatus = r.Field<string>("CurrentMaritalStatus"),
                    LaborUnionCode = r.Field<string>("LaborUnionCode"),
                    Alpha3Code = r.Field<string>("Alpha3Code"),
                    GenerationCode = r.Field<int>("GenerationCode"),
                    BirthDate = r.Field<DateTime>("BirthDate"),
                    CurrentAge = r.Field<int>("CurrentAge"),
                    CurrentState = r.Field<string>("CurrentState"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    Gender = r.Field<string>("Gender"),
                    ID = r.Field<string>("ID"),
                    PhoneOffice = r.Field<string>("PhoneOffice"),
                    RecruitmentDATE = r.Field<DateTime>("RecruitmentDATE"),
                    SeniorityDATE = r.Field<DateTime>("SeniorityDATE"),
                    SocialSecurityNumber = r.Field<string>("SocialSecurityNumber"),
                    TelephoneExtension = r.Field<string>("TelephoneExtension"),
                    EmployeeAssociationCode = r.Field<string>("EmployeeAssociationCode"),
                    InsuranceClassCode = r.Field<string>("InsuranceClassCode"),
                    Email = r.Field<string>("Email"),
                    Inferred = r.Field<bool>("Inferred"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                    ETLGuid = r.Field<Guid>("ETLGuid"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    CompanyCode = !string.IsNullOrEmpty(r.Field<string>("CompanyCode")) ? Convert.ToInt32(r.Field<string>("CompanyCode")) : 0,
                    CompanyName = !string.IsNullOrEmpty(r.Field<string>("CompanyName")) ? r.Field<string>("CompanyName") : "",
                    DepartmentCode = r.Field<string>("DepartmentCode"),
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
        /// Get OverTime Employee By Employee Code
        /// </summary>        
        /// <param name="employeeCode">employee Code</param> 
        /// <return>OverTimeEmployeeView</return>
        public OverTimeEmployeeView GetOverTimeEmployeeByEmployeeCode(string employeeCode)
        {
            try
            {
                OverTimeEmployeeView overTimeEmployeeView = new OverTimeEmployeeView();
                SqlParameter[] sqlParameters = new SqlParameter[1];
                string sqlQuery = $"SELECT TOP(1) * FROM OverTime.Employees where EmployeeCode = '{employeeCode}'";
                sqlParameters[0] = new SqlParameter("@sqlQuery", SqlDbType.NVarChar) { Value = sqlQuery };
                DataSet dataSet = Dal.QueryDataSet("OverTime.SqlQuerywithCritearea", sqlParameters);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    var result = dataSet.Tables["Table"].AsEnumerable().Select(r => new OverTimeEmployeeView
                    {
                        EmployeeCode = r.Field<string>("EmployeeCode"),
                        GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                        AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                        DivisionCode = r.Field<int>("DivisionCode"),
                        CurrentMaritalStatus = r.Field<string>("CurrentMaritalStatus"),
                        LaborUnionCode = r.Field<string>("LaborUnionCode"),
                        Alpha3Code = r.Field<string>("Alpha3Code"),
                        GenerationCode = r.Field<int>("GenerationCode"),
                        BirthDate = r.Field<DateTime>("BirthDate"),
                        CurrentAge = r.Field<int>("CurrentAge"),
                        CurrentState = r.Field<string>("CurrentState"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        Gender = r.Field<string>("Gender"),
                        ID = r.Field<string>("ID"),
                        PhoneOffice = r.Field<string>("PhoneOffice"),
                        RecruitmentDATE = r.Field<DateTime>("RecruitmentDATE"),
                        SeniorityDATE = r.Field<DateTime>("SeniorityDATE"),
                        SocialSecurityNumber = r.Field<string>("SocialSecurityNumber"),
                        TelephoneExtension = r.Field<string>("TelephoneExtension"),
                        EmployeeAssociationCode = r.Field<string>("EmployeeAssociationCode"),
                        InsuranceClassCode = r.Field<string>("InsuranceClassCode"),
                        Email = r.Field<string>("Email"),
                        Inferred = r.Field<bool>("Inferred"),
                        SearchEnabled = r.Field<bool>("SearchEnabled"),
                        Deleted = r.Field<bool>("Deleted"),
                        ETLGuid = r.Field<Guid>("ETLGuid"),
                        ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                        CompanyCode = !string.IsNullOrEmpty(r.Field<string>("CompanyCode")) ? Convert.ToInt32(r.Field<string>("CompanyCode")) : 0,
                        CompanyName = r.Field<string>("CompanyName"),
                        DepartmentCode = r.Field<string>("DepartmentCode"),
                    }).ToList();
                    if (result != null && result.Count() > 0)
                    {
                        overTimeEmployeeView = result.FirstOrDefault();
                    }
                }
                return overTimeEmployeeView;
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
        /// Get List the OverTime Record
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
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
        public List<OverTimeRecordsEntity> GetOverTimeRecordListByFilter(string geographicDivisionCode, int divisionCode, string userEmployeeCode, string employeeCode, DateTime? overtimeFromDate, DateTime? overtimeToDate, DateTime? overtimeCreatedFromDate, DateTime? overtimeCreatedToDate, int startHourFrom, int startHourTo, int endHourFrom, int endHourTo, int overTimeStatus, int overtimeNumber)
        {
            // procedureName = string.IsNullOrEmpty(procedureName) ? "OverTime.OverTimeRecordListByFilters" : procedureName;
            try
            {
                var ds = Dal.QueryDataSet("OverTime.OverTimeRecordList", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@UserEmployeeCode", userEmployeeCode),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@OvertimeFromDate", overtimeFromDate),
                    new SqlParameter("@OvertimeToDate", overtimeToDate),
                    new SqlParameter("@OvertimeCreatedFromDate", overtimeCreatedFromDate),
                    new SqlParameter("@OvertimeCreatedToDate", overtimeCreatedToDate),
                    new SqlParameter("@StartHourFrom", startHourFrom),
                    new SqlParameter("@StartHourTo", startHourTo),
                    new SqlParameter("@EndHourFrom", endHourFrom),
                    new SqlParameter("@EndHourTo", endHourTo),
                    new SqlParameter("@OverTimeStatus", overTimeStatus),
                    new SqlParameter("@OvertimeNumber", overtimeNumber)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new OverTimeRecordsEntity
                {
                    OverTimeNumber = r.Field<int>("OverTimeNumber"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    Employee = r.Field<string>("Employee"),
                    StartHour = r.Field<string>("StartHour"),
                    EndHour = r.Field<string>("EndHour"),
                    OverTimeStatusCode = r.Field<int>("OverTimeStatusCode"),
                    OverTimeStatus = r.Field<string>("OverTimeStatus"),
                    JustificationForExtraTime = r.Field<string>("JustificationForExtraTime"),
                    IsExtraHour = r.Field<bool>("IsExtraHour"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                    OvertimeDate = r.Field<DateTime>("OvertimeDate"),
                    OvertimeCreatedDate = r.Field<DateTime>("OvertimeCreatedDate"),
                    Company = r.Field<string>("Company"),
                    AssignToUser = r.Field<string>("AssignToUser")
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
