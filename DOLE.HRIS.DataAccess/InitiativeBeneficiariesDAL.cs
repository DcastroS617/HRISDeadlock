using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class InitiativeBeneficiariesDAL : IInitiativeBeneficiariesDAL<InitiativeBeneficiaries>
    {
        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<InitiativeBeneficiaries> ListByFilters(
            int? initiativeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativeBeneficiariesListByFilters", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new InitiativeBeneficiaries
                {
                    IndividualCode = r.Field<string>("IndividualCode"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationship = r.Field<string>("FamilyRelationship"),
                    Dimension = r.Field<string>("Dimension"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    //DivisionName = r.Field<string>("DivisionName"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                }).ToList();

                return new PageHelper<InitiativeBeneficiaries>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<IndividualsDeprivations> IndividualsDeprivationsByFilters(
            int? initiativeCode,
            int?   poverty,
            string gender,
            string familyRelationship,
            int?   startAge,
            int?   endAge,
            int?   startSeniority,
            int?   endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string employeeCode,
            string employeeName,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.IndividualsDeprivationsByFilters", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@Poverty", poverty),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@FamilyRelationhip", familyRelationship),
                    new SqlParameter("@StartAge", startAge),
                    new SqlParameter("@EndAge", endAge),
                    new SqlParameter("@StartSeniority", startSeniority),
                    new SqlParameter("@EndSeniority", endSeniority),
                    new SqlParameter("@StartPovertyScore", startPovertyScore),
                    new SqlParameter("@EndPovertyScore", endPovertyScore),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@EmployeeName", employeeName),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new IndividualsDeprivations
                {
                    IndividualCode = r.Field<string>("IndividualCode"),
                    //IndicatorCode = r.Field<int>("IndicatorCode"),
                    //IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    DeprivationScore = r.Field<decimal>("DeprivationScore"),
                    EmployeeSeniority = r.Field<int>("EmployeeSeniority"),
                    Gender = r.Field<string>("Gender"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationShip = r.Field<string>("FamilyRelationship"),
                    //Dimension = r.Field<string>("Dimension"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    //DivisionName = r.Field<string>("DivisionName"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
            }).ToList();

                return new PageHelper<IndividualsDeprivations>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Save the Initiative
        /// </summary>
        /// <param name="entity">Initiative</param>
        public DbaEntity InitiativeBeneficiariesSave(
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string employeeCode,
            string employeeName,
            string lastModifiedUser
            )
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("SocialResponsability.InitiativeBeneficiariesSave", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@Poverty", poverty),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@FamilyRelationhip", familyRelationship),
                    new SqlParameter("@StartAge", startAge),
                    new SqlParameter("@EndAge", endAge),
                    new SqlParameter("@StartSeniority", startSeniority),
                    new SqlParameter("@EndSeniority", endSeniority),
                    new SqlParameter("@StartPovertyScore", startPovertyScore),
                    new SqlParameter("@EndPovertyScore", endPovertyScore),
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@EmployeeName", employeeName),
                    new SqlParameter("@LastModifiedUser",lastModifiedUser),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List Initiatives by employee
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<IndividualsDeprivations> IndividualsDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.IndividualsDeprivationsByEmployee", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new IndividualsDeprivations
                {
                    IndividualCode = r.Field<string>("IndividualCode"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    DeprivationScore = r.Field<decimal>("DeprivationScore"),
                    EmployeeSeniority = r.Field<int>("EmployeeSeniority"),
                    Gender = r.Field<string>("Gender"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationShip = r.Field<string>("FamilyRelationship"),
                    Dimension = r.Field<string>("Dimension"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CompanyName = r.Field<string>("CompanyName"),
                    CostMiniZoneName = r.Field<string>("CostMiniZoneName"),
                    Poverty = r.Field<int>("Poverty"),
                    Deprived = r.Field<bool>("DeprivationClosedManagement") == true ? 1 : 0,
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                }).ToList();

                return new PageHelper<IndividualsDeprivations>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List Initiatives by employee
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<IndividualsDeprivations> HouseholdDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.HouseholdDeprivationsByEmployee", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new IndividualsDeprivations
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("IndividualName"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    DeprivationScore = r.Field<decimal>("DeprivationScore"),
                    EmployeeSeniority = r.Field<int>("EmployeeSeniority"),
                    Gender = r.Field<string>("Gender"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationShip = r.Field<string>("FamilyRelationship"),
                    Dimension = r.Field<string>("Dimension"),
                    Deprived = r.Field<bool>("DeprivationClosedManagement") == true ? 1 : 0,
                    DivisionName = r.Field<string>("DivisionName"),
                    CompanyName = r.Field<string>("CompanyName"),
                    CostMiniZoneName = r.Field<string>("CostMiniZoneName"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    //DivisionName = r.Field<string>("DivisionName"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                }).ToList();

                return new PageHelper<IndividualsDeprivations>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<IndividualsDeprivations> DeprivationsByFilters(
            string employeeCode,
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationsByFilters", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CompanyCode", companyCode),
                    new SqlParameter("@CostFarmId", costFarmId),
                    new SqlParameter("@CoordinatorCode", coordinatorCode),
                    new SqlParameter("@IndicatorCode", indicatorCode),

                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@Poverty", poverty),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@FamilyRelationhip", familyRelationship), 
                    new SqlParameter("@StartAge", startAge),
                    new SqlParameter("@EndAge", endAge),
                    new SqlParameter("@StartSeniority", startSeniority),
                    new SqlParameter("@EndSeniority", endSeniority),
                    new SqlParameter("@StartPovertyScore", startPovertyScore),
                    new SqlParameter("@EndPovertyScore", endPovertyScore),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new IndividualsDeprivations
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    //IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    DeprivationScore = r.Field<decimal>("DeprivationScore"),
                    EmployeeSeniority = r.Field<int>("EmployeeSeniority"),
                    Gender = r.Field<string>("Gender"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationShip = r.Field<string>("FamilyRelationship"),
                    Deprived = r.Field<bool>("DeprivationClosedManagement") == true ? 1 : 0,
                    //Dimension = r.Field<string>("Dimension"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    //DivisionName = r.Field<string>("DivisionName"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                }).ToList();

                return new PageHelper<IndividualsDeprivations>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<IndividualsDeprivations> ClosedDeprivationsByFilters(
            string employeeCode,
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.ClosedDeprivationsByFilters", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", employeeCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CompanyCode", companyCode),
                    new SqlParameter("@CostFarmId", costFarmId),
                    new SqlParameter("@CoordinatorCode", coordinatorCode),
                    new SqlParameter("@IndicatorCode", indicatorCode),

                    new SqlParameter("@InitiativeCode", initiativeCode),
                    new SqlParameter("@Poverty", poverty),
                    new SqlParameter("@Gender", gender),
                    new SqlParameter("@FamilyRelationhip", familyRelationship),
                    new SqlParameter("@StartAge", startAge),
                    new SqlParameter("@EndAge", endAge),
                    new SqlParameter("@StartSeniority", startSeniority),
                    new SqlParameter("@EndSeniority", endSeniority),
                    new SqlParameter("@StartPovertyScore", startPovertyScore),
                    new SqlParameter("@EndPovertyScore", endPovertyScore),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                }, 360);

                var page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new IndividualsDeprivations
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    DeprivationCode = r.Field<int>("DeprivationCode"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    //IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    IndividualName = r.Field<string>("IndividualName"),
                    DeprivationScore = r.Field<decimal>("DeprivationScore"),
                    EmployeeSeniority = r.Field<int>("EmployeeSeniority"),
                    Gender = r.Field<string>("Gender"),
                    Age = r.Field<int>("Age"),
                    FamilyRelationShip = r.Field<string>("FamilyRelationship"),
                    CompanyName = r.Field<string>("DeprivationStatusDesSpanish"),
                    Deprived = r.Field<bool>("DeprivationClosedManagement") == true ? 1 : 0,
                    //Dimension = r.Field<string>("Dimension"),
                    //DivisionCode = r.Field<int>("DivisionCode"),
                    //DivisionName = r.Field<string>("DivisionName"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    //GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                }).ToList();

                return new PageHelper<IndividualsDeprivations>(result, page.TotalResults, pageNumber, page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
