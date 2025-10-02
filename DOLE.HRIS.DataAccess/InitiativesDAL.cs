using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class InitiativesDal : IInitiativesDAL<InitiativeEntity>
    {
        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="companyCode">Company Code</param>
        /// <param name="costFarmId">Cost Farm ID</param>
        /// <param name="indicatorCode">Indicator Code</param>
        /// <param name="coordinatorCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        public PageHelper<InitiativeEntity> ListByFilters(
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativesListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@CompanyCode", companyCode),
                    new SqlParameter("@CostFarmId", costFarmId),
                    new SqlParameter("@IndicatorCode", indicatorCode),
                    new SqlParameter("@CoordinatorCode", coordinatorCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new InitiativeEntity
                {
                    InitiativeCode = r.Field<long>("InitiativeCode"),
                    InitiativeName = r.Field<string>("InitiativeName"),
                    StartDate = r.Field<DateTime>("StartDate"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    Budget = r.Field<decimal>("Budget"),
                    Beneficiaries = r.Field<int>("Beneficiaries"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    Dimension = r.Field<string>("Dimension"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    //CompanyCode = r.Field<int>("CompanyCode"),
                    //CompanyName = r.Field<string>("CompanyName"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    //CostFarmId = r.Field<string>("CostFarmId"),
                    //CostFarmName = r.Field<string>("CostFarmName"),
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    CoordinatorName = r.Field<string>("CoordinatorName"),
                    Description = r.Field<string>("Description"),
                    GeneralObjective = r.Field<string>("GeneralObjective"),
                    BeneficiariesProfile = r.Field<string>("BeneficiariesProfile"),
                    InvestedHours = r.Field<int>("InvestedHours"),
                    BMPIAsociated = r.Field<bool>("BMPIAssociated"),
                    Notes = r.Field<string>("Notes")
                }).ToList();

                return new PageHelper<InitiativeEntity>(result, page.TotalResults, pageNumber, page.PageSize);
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
        /// <param name="initiativeCode">Division Code</param>
        public InitiativeEntity ListByKey(
            int? initiativeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativesListByKey", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode)
                }, 360);


                var result = ds.Tables[0].AsEnumerable().Select(r => new InitiativeEntity
                {
                    InitiativeCode = r.Field<long>("InitiativeCode"),
                    InitiativeName = r.Field<string>("InitiativeName"),
                    StartDate = r.Field<DateTime>("StartDate"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    Budget = r.Field<decimal>("Budget"),
                    Beneficiaries = r.Field<int>("Beneficiaries"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    Dimension = r.Field<string>("Dimension"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    //CompanyCode = r.Field<int>("CompanyCode"),
                    CompanyName = r.Field<string>("CompanyName"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    //CostFarmId = r.Field<string>("CostFarmId"),
                    CostFarmName = r.Field<string>("CostFarmName"),
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    CoordinatorName = r.Field<string>("CoordinatorName"),
                    Description = r.Field<string>("Description"),
                    GeneralObjective = r.Field<string>("GeneralObjective"),
                    BeneficiariesProfile = r.Field<string>("BeneficiariesProfile"),
                    InvestedHours = r.Field<int>("InvestedHours"),
                    BMPIAsociated = r.Field<bool>("BMPIAssociated"),
                    Notes = r.Field<string>("Notes")
                }).FirstOrDefault();

                return result;
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
        /// <param name="initiativeCode">Division Code</param>
        public Tuple<InitiativeEntity, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>> ListByKeyTuple(
            int? initiativeCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativesListByKey", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", initiativeCode)
                }, 360);


                var result = ds.Tables[0].AsEnumerable().Select(r => new InitiativeEntity
                {
                    InitiativeCode = r.Field<long>("InitiativeCode"),
                    InitiativeName = r.Field<string>("InitiativeName"),
                    StartDate = r.Field<DateTime>("StartDate"),
                    EndDate = r.Field<DateTime>("EndDate"),
                    Budget = r.Field<decimal>("Budget"),
                    Beneficiaries = r.Field<int>("Beneficiaries"),
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName"),
                    Dimension = r.Field<string>("Dimension"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    //CompanyCode = r.Field<int>("CompanyCode"),
                    CompanyName = r.Field<int>("CompanyName").ToString(),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AccountingGeographicDivisionCode = r.Field<string>("AccountingGeographicDivisionCode"),
                    //CostFarmId = r.Field<string>("CostFarmId"),
                    CostFarmName = r.Field<int>("CostFarmName").ToString(),
                    CoordinatorCode = r.Field<int>("CoordinatorCode"),
                    CoordinatorName = r.Field<string>("CoordinatorName"),
                    Description = r.Field<string>("Description"),
                    GeneralObjective = r.Field<string>("GeneralObjective"),
                    BeneficiariesProfile = r.Field<string>("BeneficiariesProfile"),
                    InvestedHours = r.Field<int>("InvestedHours"),
                    BMPIAsociated = r.Field<bool>("BMPIAssociated"),
                    Notes = r.Field<string>("Notes")
                }).FirstOrDefault();

                var resultCostZones = ds.Tables[1].AsEnumerable().Select(r => new MatrixTargetByCostZonesEntity
                {
                    CostZoneId = r.Field<string>("CostZonesID"),
                }).ToList();

                var resultCostMiniZones = ds.Tables[2].AsEnumerable().Select(r => new MatrixTargetByCostMiniZonesEntity
                {
                    CostMiniZoneId = r.Field<string>("CostMiniZoneID"),
                }).ToList();

                var resultCostFarms = ds.Tables[3].AsEnumerable().Select(r => new MatrixTargetByCostFarmsEntity
                {
                    CostFarmId = r.Field<string>("CostFarmID"),
                }).ToList();


                return new Tuple<InitiativeEntity, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>>(result, resultCostZones, resultCostMiniZones, resultCostFarms);
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
        /// List all Indicators
        /// </summary>
        /// <param name="divisionCode">Division Code</param>
        /// <returns>List of Indicator Entities</returns>
        public List<InitiativeEntity> ListAll(int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.InitiativesList", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new InitiativeEntity
                {
                    InitiativeCode = r.Field<long>("InitiativeCode"),
                    InitiativeName = r.Field<string>("InitiativeName")                    
                }).ToList();

                return result;
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
        public DbaEntity InitiativesSave(InitiativeEntity entity, DataTable costZones, DataTable costMiniZones, DataTable costFarms, bool costZonesMarkAll, bool costMiniZonesMarkAll, bool costFarmsMarkAll)
        {
            try
            {
                string CostZones = JsonConvert.SerializeObject(costZones);
                string CostMiniZones = JsonConvert.SerializeObject(costMiniZones);
                string CostFarms = JsonConvert.SerializeObject(costFarms);

                var ds = Dal.TransactionScalarTuple("SocialResponsability.InitiativesSave", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", entity.InitiativeCode == 0 ? (object)DBNull.Value : entity.InitiativeCode),
                    new SqlParameter("@InitiativeName", entity.InitiativeName),
                    new SqlParameter("@IndicatorCode", entity.IndicatorCode.HasValue ? (object)entity.IndicatorCode : DBNull.Value),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),

                    new SqlParameter("@CostZonesMarkAll",costZonesMarkAll),
                    new SqlParameter("@CostZones",CostZones),

                    new SqlParameter("@CostMiniZonesMarkAll",costMiniZonesMarkAll),
                    new SqlParameter("@CostMiniZones",CostMiniZones),

                    new SqlParameter("@CostFarmsMarkAll",costFarmsMarkAll),
                    new SqlParameter("@CostFarms",CostFarms),

                    new SqlParameter("@CoordinatorCode", entity.CoordinatorCode),
                    new SqlParameter("@StartDate", entity.StartDate),
                    new SqlParameter("@EndDate", entity.EndDate),
                    new SqlParameter("@Budget", entity.Budget),
                    new SqlParameter("@Beneficiaries", entity.Beneficiaries),
                    new SqlParameter("@Description", entity.Description),
                    new SqlParameter("@GeneralObjective", entity.GeneralObjective),
                    new SqlParameter("@BeneficiariesProfile", entity.BeneficiariesProfile),
                    new SqlParameter("@InvestedHours", entity.InvestedHours),
                    new SqlParameter("@BMPIAssociated", entity.BMPIAsociated),
                    new SqlParameter("@LastModifiedUser", entity.LastModifiedUser),
                    new SqlParameter("@Notes", string.IsNullOrEmpty(entity.Notes) ? (object)DBNull.Value : entity.Notes)
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
        /// Save the Initiative
        /// </summary>
        /// <param name="entity">Initiative</param>
        public DbaEntity InitiativesDeactivate(InitiativeEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("SocialResponsability.InitiativeDeactivate", new SqlParameter[] {
                    new SqlParameter("@InitiativeCode", entity.InitiativeCode == 0 ? (object)DBNull.Value : entity.InitiativeCode)
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
    }
}
