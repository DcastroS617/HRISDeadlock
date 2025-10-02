using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using DOLE.HRIS.Application.DataAccess.Interfaces;

namespace DOLE.HRIS.Application.DataAccess
{
    public class WorkingTimeHoursByDepartmentEmployeesDAL : IWorkingTimeHoursByDepartmentEmployeesDAL<WorkingTimeHoursByDepartmentEmployeesEntity>
    {
        /// <summary>
        /// Get List the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param> 
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeHoursByDepartmentEmployeesEntity List</return>
        public PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> GetWorkingTimeHoursByDepartmentEmployeesList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingTimeHoursByDepartmentEmployeesListByFilters", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@DepartmentCode", departmentCode),
                    new SqlParameter("@EmployeeCode", employeeCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new WorkingTimeHoursByDepartmentEmployeesEntity
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
                    WorkingTimeType = r.Field<string>("WorkingTimeType"),
                    SecondStartHour = r.Field<string>("SecondStartHour"),
                    SecondEndHour = r.Field<string>("SecondEndHour"),
                    RestDay = r.Field<string>("RestDay"),
                    Department = r.Field<string>("Department"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Working Time Hours By Department Employees By Id
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">working TimeHours By Department Employees ID</param>
        /// <returns>WorkingTimeHoursByDepartmentEmployeesEntity</returns>
        public WorkingTimeHoursByDepartmentEmployeesEntity GetWorkingTimeHoursByDepartmentEmployeesById(int workingTimeHoursByDepartmentEmployeesID)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.WorkingTimeHoursByDepartmentEmployeesByID", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeHoursByDepartmentEmployeesID", workingTimeHoursByDepartmentEmployeesID)
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
                    SecondStartHour = r.Field<string>("SecondStartHour"),
                    SecondEndHour = r.Field<string>("SecondEndHour"),
                    RestDay = r.Field<string>("RestDay"),
                    Department = r.Field<string>("Department"),
                    EmployeeName = r.Field<string>("EmployeeName"),
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
        /// Add Working Time Hours By Department Employees Entity
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployees">working TimeHours By Department Employees </param>
        /// <returns>bool</returns>
        public bool AddWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeHoursByDepartmentEmployeesAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",workingTimeHoursByDepartmentEmployees.GeographicDivisionCode),
                    new SqlParameter("@DepartmentCode",workingTimeHoursByDepartmentEmployees.DepartmentCode),
                    new SqlParameter("@EmployeeCode",workingTimeHoursByDepartmentEmployees.EmployeeCode),
                    new SqlParameter("@DivisionCode",workingTimeHoursByDepartmentEmployees.DivisionCode),
                    new SqlParameter("@StartDate",workingTimeHoursByDepartmentEmployees.StartDate),
                    new SqlParameter("@EndDate",workingTimeHoursByDepartmentEmployees.EndDate),
                    new SqlParameter("@StartHour",workingTimeHoursByDepartmentEmployees.StartHour),
                    new SqlParameter("@EndHour",workingTimeHoursByDepartmentEmployees.EndHour),
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeHoursByDepartmentEmployees.WorkingTimeTypeCode),
                    new SqlParameter("@RestDay",workingTimeHoursByDepartmentEmployees.RestDay),
                    new SqlParameter("@SecondStartHour",workingTimeHoursByDepartmentEmployees.SecondStartHour),
                    new SqlParameter("@SecondEndHour",workingTimeHoursByDepartmentEmployees.SecondEndHour),
                    new SqlParameter("@CreatedBy",workingTimeHoursByDepartmentEmployees.CreatedBy),
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
        /// Update Working Time Hours By Department Employees Entity
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployees">working TimeHours By Department Employees </param>
        /// <returns>bool</returns>
        public bool UpdateWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeHoursByDepartmentEmployeesUpdate", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeHoursByDepartmentEmployeesId",workingTimeHoursByDepartmentEmployees.WorkingTimeHoursByDepartmentEmployeesID),
                    new SqlParameter("@StartDate",workingTimeHoursByDepartmentEmployees.StartDate),
                    new SqlParameter("@EndDate",workingTimeHoursByDepartmentEmployees.EndDate),
                    new SqlParameter("@StartHour",workingTimeHoursByDepartmentEmployees.StartHour),
                    new SqlParameter("@EndHour",workingTimeHoursByDepartmentEmployees.EndHour),
                    new SqlParameter("@RestDay",workingTimeHoursByDepartmentEmployees.RestDay),
                    new SqlParameter("@SecondStartHour",workingTimeHoursByDepartmentEmployees.SecondStartHour),
                    new SqlParameter("@SecondEndHour",workingTimeHoursByDepartmentEmployees.SecondEndHour),
                    new SqlParameter("@WorkingTimeTypeCode",workingTimeHoursByDepartmentEmployees.WorkingTimeTypeCode),
                    new SqlParameter("@LastModifiedUser",workingTimeHoursByDepartmentEmployees.LastModifiedUser)
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
        /// Delete Working Time Hours By Department Employees Entity
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployees">working TimeHours By Department Employees </param>
        /// <returns>bool</returns>
        public bool DeleteWorkingTimeHoursByDepartmentEmployees(int workingTimeHoursByDepartmentEmployeesID)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.WorkingTimeHoursByDepartmentEmployeesByDelete", new SqlParameter[] {
                    new SqlParameter("@WorkingTimeHoursByDepartmentEmployeesID",workingTimeHoursByDepartmentEmployeesID)
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
    }
}
