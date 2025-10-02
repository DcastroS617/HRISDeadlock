using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class RolesByDepartmentEmployeeDAL : IRolesByDepartmentEmployeeDAL<RolesByDepartmentEmployeeEntity>
    {
        /// <summary>
        /// Get List the Roles By Department Employee
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The RolesByDepartmentEmployeeEntity List</return>
        public PageHelper<RolesByDepartmentEmployeeEntity> GetRolesByDepartmentEmployeeList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.RolesByDepartmentEmployeeListByFilters", new SqlParameter[] {
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new RolesByDepartmentEmployeeEntity
                {
                    RolesByDepartmentEmployeeID = r.Field<int>("RolesByDepartmentEmployeeID"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    RoleApproversCode = r.Field<int>("RoleApproversCode"),
                    RoleApproversDescription = r.Field<string>("RoleApproversDescription"),
                    Department = r.Field<string>("Department"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<RolesByDepartmentEmployeeEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Roles By Department Employee By Id
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">roles By Department Employee ID</param>
        /// <returns>RolesByDepartmentEmployeeEntity</returns>
        public RolesByDepartmentEmployeeEntity GetRolesByDepartmentEmployeeById(int rolesByDepartmentEmployeeID)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.RolesByDepartmentEmployeeById", new SqlParameter[] {
                    new SqlParameter("@RolesByDepartmentEmployeeID", rolesByDepartmentEmployeeID)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new RolesByDepartmentEmployeeEntity
                {
                    RolesByDepartmentEmployeeID = r.Field<int>("RolesByDepartmentEmployeeID"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    RoleApproversCode = r.Field<int>("RoleApproversCode"),
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
        /// Get List the Roles By Department Employee
        /// </summary>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The RolesByDepartmentEmployeeEntity List</return>
        public List<RolesByDepartmentEmployeeEntity> GetRolesByDepartmentEmployeeList(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployeeEntity)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.RolesByDepartmentEmployeeListByDeparmentCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", rolesByDepartmentEmployeeEntity.GeographicDivisionCode),
                    new SqlParameter("@DepartmentCode", rolesByDepartmentEmployeeEntity.DepartmentCode),
                    new SqlParameter("@RoleApproversCode", rolesByDepartmentEmployeeEntity.RoleApproversCode),
                    new SqlParameter("@EmployeeCode", rolesByDepartmentEmployeeEntity.EmployeeCode),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new RolesByDepartmentEmployeeEntity
                {
                    RolesByDepartmentEmployeeID = r.Field<int>("RolesByDepartmentEmployeeID"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    RoleApproversCode = r.Field<int>("RoleApproversCode"),
                    RoleApproversDescription = r.Field<string>("RoleApproversDescription"),
                    Department = r.Field<string>("Department"),
                    EmployeeName = r.Field<string>("EmployeeName"),
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


        /// <summary>
        /// Save the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param> 
        public bool AddRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.RolesByDepartmentEmployeeAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",rolesByDepartmentEmployee.GeographicDivisionCode),
                    new SqlParameter("@DepartmentCode",rolesByDepartmentEmployee.DepartmentCode),
                    new SqlParameter("@EmployeeCode",rolesByDepartmentEmployee.EmployeeCode),
                    new SqlParameter("@RoleApproversCode",rolesByDepartmentEmployee.RoleApproversCode),
                    new SqlParameter("@CreatedBy",rolesByDepartmentEmployee.CreatedBy)
                });
                return result >= 1;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjInvalidData), sqlex);
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
        /// Update the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param> 
        public bool UpdateRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.RolesByDepartmentEmployeeUpdate", new SqlParameter[] {
                    new SqlParameter("@RolesByDepartmentEmployeeID",rolesByDepartmentEmployee.RolesByDepartmentEmployeeID),
                    new SqlParameter("@GeographicDivisionCode",rolesByDepartmentEmployee.GeographicDivisionCode),
                    new SqlParameter("@DepartmentCode",rolesByDepartmentEmployee.DepartmentCode),
                    new SqlParameter("@EmployeeCode",rolesByDepartmentEmployee.EmployeeCode),
                    new SqlParameter("@RoleApproversCode",rolesByDepartmentEmployee.RoleApproversCode),
                    new SqlParameter("@LastModifiedUser",rolesByDepartmentEmployee.LastModifiedUser)
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
        /// Delete the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param> 
        public bool DeleteRolesByDepartmentEmployee(int RolesByDepartmentEmployeeID)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.RolesByDepartmentEmployeeDelete", new SqlParameter[] {
                    new SqlParameter("@RolesByDepartmentEmployeeID",RolesByDepartmentEmployeeID)
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
        /// List the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param> 
        public List<RoleApproversEntity> GetRoleApproversListForDropdown()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.RoleApproversForDropdown", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new RoleApproversEntity
                {
                    RoleApproversCode = r.Field<int>("RoleApproversCode"),
                    RoleApproversDescription = r.Field<string>("RoleApproversDescription"),
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

        /// <summary>
        /// Get the Roles By Department Employee
        /// </summary>
        public List<Departments> GetDepartmentListForDropdown()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.DepartmentListForDropdown", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new Departments
                {
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    Department = r.Field<string>("Department")
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
