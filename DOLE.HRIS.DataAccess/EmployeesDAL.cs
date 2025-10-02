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
    public class EmployeesDal : IEmployeesDal<EmployeeEntity>
    {
        /// <summary>
        /// Filter the employees by the geographic division, division and Employee code or ID
        /// </summary>
        /// <param name="entity">The entity to filter</param>
        /// <returns>A list of employees that meet the filters</returns>
        public List<EmployeeEntity> FilterByGeographicDivisionAndEmployeeCode(EmployeeEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesFilterByGeographicDivisionAndEmployeeCode", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@EmployeeCode", entity.EmployeeCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ID = r.Field<string>("ID"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CurrentState = r.Field<string>("CurrentState"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CostCenter = r.Field<string>("CenterCost"),
                    NominalClassId = r.Field<string>("PayrollClassCode"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }
             
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByDivisionByDepartment(int divisionCode, string geographicDivisionCode, string departmentCode = null)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeeListForDropdown", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                     new SqlParameter("@DepartmentCode", departmentCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the inactive employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The inactive employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByInactiveDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesInactiveListByDivision", new SqlParameter[] {
                    new SqlParameter("@divisionCode", divisionCode),
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CostCenter = r.Field<string>("CenterCost"),
                    NominalClassId = r.Field<string>("PayrollClassCode"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by struct by farm or nominal class
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        public PageHelper<EmployeeEntity> ListByStruct(string geographicDivisionCode, int structBy, DataTable participants, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass, DataTable costCenters, string employee, int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesListByStruct", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                    new SqlParameter("@StructBy", structBy),
                    new SqlParameter("@Participants", participants),
                    new SqlParameter("@CostZones", costZones),
                    new SqlParameter("@CostMiniZones", costMiniZones),
                    new SqlParameter("@CostFarms", costFarms),
                    new SqlParameter("@Companies", companies),
                    new SqlParameter("@NominalClass", nominalClass),
                    new SqlParameter("@CostCenters", costCenters),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@Employee",employee),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CostCenter = r.Field<string>("CostsCenterCode"),
                    NominalClassId = r.Field<string>("PayrollClassCode"),
                }).ToList();

                return new PageHelper<EmployeeEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// Filter the employee by its employee code and geographic division
        /// </summary>
        /// <param name="entity">The entity to filter</param>
        /// <returns>The employee that meet the filters</returns>
        public EmployeeEntity ListByEmployeeCodeGeographicDivision(EmployeeEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesListByEmployeeCodeGeographicDivision", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", entity.EmployeeCode),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    ID = r.Field<string>("ID"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    BirthDate = r.Field<DateTime>("BirthDate"),
                    CurrentMaritalStatus = r.Field<string>("CurrentMaritalStatus"),
                    Gender = r.Field<string>("Gender"),
                    Alpha3Code = r.Field<string>("Alpha3Code"),
                    Nationality = r.Field<string>("Nationality"),
                    CompanyID = r.Field<int>("CompanyID"),
                    CompanyName = r.Field<string>("CompanyName"),
                    CostFarmID = r.Field<string>("CostFarmID"),
                    CostFarmName = r.Field<string>("CostFarmName"),
                    RecruitmentDate = r.Field<DateTime>("RecruitmentDATE"),
                    SeniorityDate = r.Field<DateTime>("SeniorityDATE"),
                    PhoneOffice = r.Field<string>("PhoneOffice"),
                    TelephoneExtension = r.Field<string>("TelephoneExtension"),
                    DepartmentCode = r.Field<string>("DepartmentCode"),
                    DepartmentName = r.Field<string>("Department"),
                    PositionName = r.Field<string>("PositionName"),
                    Email = r.Field<string>("Email"),
                    Seat = r.Field<int>("Seat")
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }
       
        /// <summary>
        /// List the primary key of the employee that meet the email filter
        /// </summary>
        /// <param name="email">The employee email</param>
        /// <returns>The employee information</returns>
        public EmployeeEntity ListKeyByEmail(string email)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesListKeyByEmail", new SqlParameter[] {
                    new SqlParameter("@Email", email),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    CountryId = r.Field<string>("CountryID"),
                    DivisionName = r.Field<string>("DivisionName"),
                }).FirstOrDefault();

                return result;     
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employee filtering by Active Directory Usser Account
        /// </summary>
        /// <param name="userAccount">The usser account</param>
        /// <returns>An employee</returns>
        public EmployeeEntity ListEmployeeByActiveDirectoryUserAccount(string userAccount, string email)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeesListByActiveDirectoryUserAccountV2", new SqlParameter[] {
                    new SqlParameter("@UserAccount", userAccount),
                    new SqlParameter("@Email", email)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ID = r.Field<string>("ID"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    CurrentState = r.Field<string>("CurrentState"),
                    Email = r.Field<string>("Email"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    Alpha3Code = r.Field<string>("CountryID")
                }).FirstOrDefault();

                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjEmployees), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjEmployeesList, ex);
                }
            }
        }

    }
}