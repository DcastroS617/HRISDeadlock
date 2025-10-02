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
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class MatrixTargetDal : IMatrixTargetDal
    {
        /// <summary>
        /// List the matrix target by the given filters 
        /// </summary>
        /// <param name="entity">The Matrix target</param>
        /// <param name="lang">Language</param>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public PageHelper<MatrixTargetEntity> MatrixTargetByFilter(MatrixTargetEntity entity, string lang, int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MatrixTargetByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@MatrixTargetCode",entity.MatrixTargetCode),
                    new SqlParameter("@MatrixTargetName",entity.MatrixTargetName),
                    new SqlParameter("@StructBy",entity.StructBy),
                    new SqlParameter("@lang",lang),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new MatrixTargetEntity
                {
                    MatrixTargetId = r.Field<int?>("MatrixTargetId"),
                    MatrixTargetCode = r.Field<string>("MatrixTargetCode"),
                    MatrixTargetName = r.Field<string>("MatrixTargetName"),
                    DivisionName = r.Field<string>("DivisionName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    StructBy = r.Field<int?>("StructBy"),
                    IsRegional = r.Field<bool>("IsRegional")
                }).ToList();

                return new PageHelper<MatrixTargetEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Tuple the matrix target
        /// </summary>
        /// <param name="entity">The Matrix target</param>
        public Tuple<MatrixTargetEntity, List<MatrixTargetByDivisionsEntity>, List<MatrixTargetByCompaniesEntity>, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>, List<MatrixTargetByNominalClassEntity>> MatrixTargetById(MatrixTargetEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MatrixTargetById", new SqlParameter[] {
                    new SqlParameter("@MatrixTargetId",entity.MatrixTargetId),
                    new SqlParameter("@MatrixTargetCode",entity.MatrixTargetCode)
                });

                var resultEdit = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetEntity
                {
                    MatrixTargetId = r.Field<int?>("MatrixTargetId"),
                    MatrixTargetCode = r.Field<string>("MatrixTargetCode"),
                    MatrixTargetName = r.Field<string>("MatrixTargetName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    StructBy = r.Field<int>("StructBy"),
                }).FirstOrDefault();

                var resultDivisions = ds.Tables[1].AsEnumerable().Select(r => new MatrixTargetByDivisionsEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                }).ToList();

                var resultCostZones = ds.Tables[2].AsEnumerable().Select(r => new MatrixTargetByCostZonesEntity
                {
                    CostZoneId = r.Field<string>("CostZonesID"),
                }).ToList();

                var resultCostMiniZones = ds.Tables[3].AsEnumerable().Select(r => new MatrixTargetByCostMiniZonesEntity
                {
                    CostMiniZoneId = r.Field<string>("CostMiniZoneID"),
                }).ToList();

                var resultCostFarms = ds.Tables[4].AsEnumerable().Select(r => new MatrixTargetByCostFarmsEntity
                {
                    CostFarmId = r.Field<string>("CostFarmID"),
                }).ToList();

                var resultCompanies = ds.Tables[5].AsEnumerable().Select(r => new MatrixTargetByCompaniesEntity
                {
                    CompanyName = r.Field<string>("CompanyID"),
                }).ToList();

                var resultNominalClass = ds.Tables[6].AsEnumerable().Select(r => new MatrixTargetByNominalClassEntity
                {
                    NominalClassId = r.Field<string>("NominalClassId"),
                }).ToList();

                return new Tuple<MatrixTargetEntity, List<MatrixTargetByDivisionsEntity>, List<MatrixTargetByCompaniesEntity>, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>, List<MatrixTargetByNominalClassEntity>>(resultEdit, resultDivisions, resultCompanies, resultCostZones, resultCostMiniZones, resultCostFarms, resultNominalClass);
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
        /// Add the matrix target
        /// </summary>
        /// <param name="entity">The Matrix target</param>
        /// <param name="divisions">The list Nominal Class</param>
        /// <param name="costZones">The list cost zones</param>
        /// <param name="costMiniZones">The list cost mini zones</param>
        /// <param name="costFarms">The list cost farms</param>
        /// <param name="companies">The list companies</param>
        /// <param name="nominalClass">The list Nominal Class</param>
        public DbaEntity MatrixTargetAdd(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass)
        {
            try
            {
                string DivisionCodeList = JsonConvert.SerializeObject(divisions);
                string CostZones = JsonConvert.SerializeObject(costZones);
                string CostMiniZones = JsonConvert.SerializeObject(costMiniZones);
                string CostFarms = JsonConvert.SerializeObject(costFarms);
                string Companies = JsonConvert.SerializeObject(companies);
                string NominalClass = JsonConvert.SerializeObject(nominalClass);

                var ds = Dal.TransactionScalarTuple("Training.MatrixTargetAdd", new SqlParameter[] {
                    new SqlParameter("@MatrixTargetCode",entity.MatrixTargetCode),
                    new SqlParameter("@MatrixTargetName",entity.MatrixTargetName),
                    new SqlParameter("@StructBy",entity.StructBy),
                    new SqlParameter("@IsRegional",entity.IsRegional),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),

                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@DivisionCodeList",DivisionCodeList),

                    new SqlParameter("@CostZonesMarkAll",entity.CostZonesMarkAll),
                    new SqlParameter("@CostZones",CostZones),

                    new SqlParameter("@CostMiniZonesMarkAll",entity.CostMiniZonesMarkAll),
                    new SqlParameter("@CostMiniZones",CostMiniZones),

                    new SqlParameter("@CostFarmsMarkAll",entity.CostFarmsMarkAll),
                    new SqlParameter("@CostFarms",CostFarms),

                    new SqlParameter("@CompaniesMarkAll",entity.CompaniesMarkAll),
                    new SqlParameter("@Companies",Companies),

                    new SqlParameter("@NominalClassMarkAll",entity.NominalClassMarkAll),
                    new SqlParameter("@NominalClass",NominalClass),
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
        /// Edit the matrix target
        /// </summary>
        /// <param name="entity">The Matrix target</param>
        /// <param name="divisions">The list Nominal Class</param>
        /// <param name="costZones">The list cost zones</param>
        /// <param name="costMiniZones">The list cost mini zones</param>
        /// <param name="costFarms">The list cost farms</param>
        /// <param name="companies">The list companies</param>
        /// <param name="nominalClass">The list Nominal Class</param>
        public DbaEntity MatrixTargetEdit(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass)
        {
            try
            {
                string DivisionCodeList = JsonConvert.SerializeObject(divisions);
                string CostZones = JsonConvert.SerializeObject(costZones);
                string CostMiniZones = JsonConvert.SerializeObject(costMiniZones);
                string CostFarms = JsonConvert.SerializeObject(costFarms);
                string Companies = JsonConvert.SerializeObject(companies);
                string NominalClass = JsonConvert.SerializeObject(nominalClass);

                var ds = Dal.TransactionScalarTuple("Training.MatrixTargetEdit", new SqlParameter[] {
                    new SqlParameter("@MatrixTargetId",entity.MatrixTargetId),
                    new SqlParameter("@MatrixTargetName",entity.MatrixTargetName),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@StructBy",entity.StructBy),
                    new SqlParameter("@IsRegional",entity.IsRegional),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),

                    new SqlParameter("@DivisionCodeList",DivisionCodeList),

                    new SqlParameter("@CostZonesMarkAll",entity.CostZonesMarkAll),
                    new SqlParameter("@CostZones",CostZones),

                    new SqlParameter("@CostMiniZonesMarkAll",entity.CostMiniZonesMarkAll),
                    new SqlParameter("@CostMiniZones",CostMiniZones),

                    new SqlParameter("@CostFarmsMarkAll",entity.CostFarmsMarkAll),
                    new SqlParameter("@CostFarms",CostFarms),

                    new SqlParameter("@CompaniesMarkAll",entity.CompaniesMarkAll),
                    new SqlParameter("@Companies",Companies),

                    new SqlParameter("@NominalClassMarkAll",entity.NominalClassMarkAll),
                    new SqlParameter("@NominalClass",NominalClass),
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
        /// Desactive the matrix target
        /// </summary>
        /// <param name="entity"></param>
        public DbaEntity MatrixTargetDeactivate(MatrixTargetEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MatrixTargetDeactivate", new SqlParameter[] {
                    new SqlParameter("@MatrixTargetId",entity.MatrixTargetId),
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
        /// Regional Permit the matrix target
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="UserCode"></param>
        public DbaEntity MatrixTargetRegionalPermit(MatrixTargetEntity entity, int UserCode)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MatrixTargetRegionalPermit", new SqlParameter[] {
                    new SqlParameter("@UserCode",UserCode),
                    new SqlParameter("@MatrixTargetId",entity.MatrixTargetId)
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the matrix target by cost zone by key: GeographicDivisionCode
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostZonesEntity> CostZonesListEnableByDivisions(DataTable divisions)
        {
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(divisions);
                var ds = Dal.QueryDataSet("Dole.CostZonesListEnableByDivisions", new SqlParameter[] {
                     new SqlParameter("@DivisionCodeList", JSONresult)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostZonesEntity
                {
                    CostZoneId = r.Field<string>("CostZoneReportitoID"),
                    CostZoneName = r.Field<string>("CostZoneReportitoName"),
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
        /// List the matrix target by cost mini zone by key: GeographicDivisionCode 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivision(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CostMiniZonesListEnableByDivision", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode),
                     new SqlParameter("@DivisionCode", divisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostMiniZonesEntity
                {
                    CostZoneID = r.Field<string>("CostZoneID"),
                    CostMiniZoneId = r.Field<string>("CostMiniZoneID"),
                    CostMiniZoneName = r.Field<string>("CostMiniZoneName"),
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
        /// List the matrix target by cost mini zone by key: GeographicDivisionCode 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivisions(string geographicDivisionCode, DataTable divisions)
        {
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(divisions);
                var ds = Dal.QueryDataSet("Dole.CostMiniZonesListEnableByDivisions", new SqlParameter[] {
                     new SqlParameter("@DivisionCodeList", JSONresult)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostMiniZonesEntity
                {
                    CostZoneID = r.Field<string>("CostZoneID"),
                    CostMiniZoneId = r.Field<string>("CostMiniZoneID"),
                    CostMiniZoneName = r.Field<string>("CostMiniZoneName"),
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
        /// List the matrix target by cost farms by key: GeographicDivisionCode 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivision(string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CostFarmsListEnableByDivision", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostFarmsEntity
                {
                    CostZoneID = r.Field<string>("CostZoneID"),
                    CostMiniZoneID = r.Field<string>("CostMiniZoneID"),
                    CostFarmId = r.Field<string>("CostFarmID"),
                    CostFarmName = r.Field<string>("CostFarmName"),
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
        /// List the matrix target by cost farms by key: GeographicDivisionCode 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivisions(string geographicDivisionCode, DataTable divisions)
        {
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(divisions);
                var ds = Dal.QueryDataSet("Dole.CostFarmsListEnableByDivisions", new SqlParameter[] {
                     new SqlParameter("@DivisionCodeList", JSONresult)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostFarmsEntity
                {
                    CostZoneID = r.Field<string>("CostZoneID"),
                    CostMiniZoneID = r.Field<string>("CostMiniZoneID"),
                    CostFarmId = r.Field<string>("CostFarmID"),
                    CostFarmName = r.Field<string>("CostFarmName"),
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
        /// List the companies by key: Divisions 
        /// </summary>
        /// <param name="divisions"></param>
        public List<MatrixTargetByCompaniesEntity> CompaniesListEnableByDivision(DataTable divisions)
        {
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(divisions);
                var ds = Dal.QueryDataSet("Dole.CompaniesListEnableByDivisions", new SqlParameter[] {
                     new SqlParameter("@DivisionCodeList", JSONresult)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCompaniesEntity
                {
                    CompanyID = r.Field<string>("CompanyID"),
                    CompanyName = r.Field<string>("CompanyName"),
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
        /// List the matrix target by cost mini zone by key: GeographicDivisionCode
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanie(string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.NominalClassListEnabledByDivision", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByNominalClassEntity
                {
                    CompanyCode = r.Field<string>("CompanyCode"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    NominalClassName = r.Field<string>("NominalClassName"),
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
        /// List the matrix target by cost mini zone by key: GeographicDivisionCode
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanies(DataTable divisions)
        {
            try
            {
                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(divisions);
                var ds = Dal.QueryDataSet("Dole.NominalClassListEnabledByDivisions", new SqlParameter[] {
                     new SqlParameter("@DivisionCodeList", JSONresult)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByNominalClassEntity
                {
                    CompanyCode = r.Field<string>("CompanyCode"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    NominalClassName = r.Field<string>("NominalClassName"),
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
        /// List the matrix target by cost centers by key: GeographicDivisionCode 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        public List<MatrixTargetByCostCentresEntity> CostCentersListByStruct(string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CostCentersListByStruct", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", geographicDivisionCode)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MatrixTargetByCostCentresEntity
                {
                    CostZoneID = r.Field<string>("CostZoneID"),
                    CostMiniZoneID = r.Field<string>("CostMiniZoneID"),
                    CostFarmID = r.Field<string>("CostFarmID"),

                    CompanyCode = r.Field<string>("CompanyCode"),
                    NominalClassId = r.Field<string>("PayrollClassCode"),

                    CostCenterID = r.Field<string>("CostCenterID"),
                    CostCenterName = r.Field<string>("CostCenterName"),
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
    }
}
