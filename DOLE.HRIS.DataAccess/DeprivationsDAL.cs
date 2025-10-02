using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DeprivationsDAL : IDeprivationsDAL<DeprivationEntity>
    {
        /// <summary>
        /// List Individuals by Filters
        /// </summary>
        /// <param name="indicatorCode">Indicator Code</param>
        /// <param name="divisionCode">Division Name</param>
        /// <param name="companyCode">Company Name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Individual Entities</returns>
        public PageHelper<DeprivationEntity> ListByFilters(
            int? indicatorCode,
            string divisionCode,
            string companyCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationsListByFilters", new SqlParameter[] {
                    new SqlParameter("@IndicatorCode", indicatorCode),
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CompanyCode", companyCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new DeprivationEntity
                {
                    IndividualHID = r.Field<long>("IndividualHID"),
                    IndividualCode = r.Field<string>("IndividualCode"),
                    IndividualName = r.Field<string>("IndividualName"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    Dimension = r.Field<string>("Dimension"),
                    Deprived = r.Field<int>("Deprived"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CompanyName = r.Field<string>("CompanyName"),
                    CostMiniZoneName = r.Field<string>("CostMiniZoneName"),
                    IndividualType = r.Field<string>("IndividualType")
                }).ToList();

                return new PageHelper<DeprivationEntity>(result, page.TotalResults, pageNumber, page.PageSize);
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
        /// List Surveys by Filters
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <returns>List of Survey Entities</returns>
        public PageHelper<HouseholdDeprivationEntity> ListByEmployee(
            string employeeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.DeprivationHouseholdListByEmployee", new SqlParameter[] {
                    new SqlParameter("@EmployeeCode", employeeCode),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new HouseholdDeprivationEntity
                {
                    SurveyCode = r.Field<long>("SurveyCode"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    DeprivatedHousehold = r.Field<string>("DeprivatedHousehold"),
                    DeprivationClosed = r.Field<string>("DeprivationClosed"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    Dimension = r.Field<string>("Dimension")
                }).ToList();

                return new PageHelper<HouseholdDeprivationEntity>(result, 16, 1, 16);
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