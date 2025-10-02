using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
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
    public class MapCatalogPositionsDal : IMapCatalogPositionsDal
    {
        /// <summary>
        /// List the map catalog position by the given filters
        /// </summary>
        /// <param name="entity">The map catalog position</param>
        /// <param name="Divisioncode">Division Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        public PageHelper<MapCatalogPositionsEntity> MapCatalogPositionsListByFilter(MapCatalogPositionsEntity entity,int Divisioncode,  string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.MapCatalogPositionsListByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@PositionCode",entity.PositionCode),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new MapCatalogPositionsEntity
                {
                    MapCatalogPositionsId = r.Field<int?>("MapCatalogPositionsId"),
                    CategoryFTEId = r.Field<int?>("CategoryFTEId"),
                    CategoryFTE = {  CategoryFTEDescripcion =  r.Field<string>("CategoryFTEDescripcion") },
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    Position = { PositionName = r.Field<string>("PositionName") },
                    CompanyIDs =  r.Field<string>("CompanyIDs"),
                    NominalClassIds =  r.Field<string>("NominalClassIds"),
                    PaymentRateCode = r.Field<short?>("PaymentRateCode")
                }).ToList();

                return new PageHelper<MapCatalogPositionsEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List tuple by map catalog position
        /// </summary>
        /// <param name="entity"></param>
        public Tuple<MapCatalogPositionsEntity, List<MapCatalogPositionsByCompaniesEntity>, List<MapCatalogPositionsByNominalClassEntity>, MapCatalogPositionsByPaymentRatesEntity> MapCatalogPositionsById(MapCatalogPositionsEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.MapCatalogPositionsById", new SqlParameter[] {
                    new SqlParameter("@MapCatalogPositionsId",entity.MapCatalogPositionsId)
                });

                var resultEdit = ds.Tables[0].AsEnumerable().Select(r => new MapCatalogPositionsEntity
                {
                    MapCatalogPositionsId = r.Field<int?>("MapCatalogPositionsId"),
                    CategoryFTEId = r.Field<int?>("CategoryFTEId"),
                    PositionCode = r.Field<string>("PositionCode")
                }).FirstOrDefault();

                var resultCompanies = ds.Tables[1].AsEnumerable().Select(r => new MapCatalogPositionsByCompaniesEntity
                {
                    CompanyID = r.Field<int?>("CompanyID"),               
                }).ToList();

                var resultNominalClass = ds.Tables[2].AsEnumerable().Select(r => new MapCatalogPositionsByNominalClassEntity
                {
                    NominalClassId = r.Field<string>("NominalClassId"),
                }).ToList();

                var resultPaymentRate =  ds.Tables[3].AsEnumerable().Select(r => new MapCatalogPositionsByPaymentRatesEntity
                {
                    PaymentRateCode = r.Field<short?>("PaymentRateCode"),
                }).FirstOrDefault();

                return new Tuple<MapCatalogPositionsEntity, List<MapCatalogPositionsByCompaniesEntity>, List<MapCatalogPositionsByNominalClassEntity>, MapCatalogPositionsByPaymentRatesEntity>(resultEdit,resultCompanies,resultNominalClass,resultPaymentRate);
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
        /// Add the map catalog positions
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Companies"></param>
        /// <param name="NominalClass"></param>
        public DbaEntity MapCatalogPositionsAdd(MapCatalogPositionsEntity entity,DataTable Companies,DataTable NominalClass)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.MapCatalogPositionsAdd", new SqlParameter[] {
                    new SqlParameter("@CategoryFTEId",entity.CategoryFTEId),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@PositionCode",entity.PositionCode),
                    new SqlParameter("@Companies",Companies),
                    new SqlParameter("@NominalClass",NominalClass),
                    new SqlParameter("@PaymentRateCode",entity.PaymentRateCode),
                });
            
                return new DbaEntity { ErrorNumber = ds.Item1,ErrorMessage = ds.Item2 };
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
        /// Edit the map catalog positions
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Companies"></param>
        /// <param name="NominalClass"></param>
        public DbaEntity MapCatalogPositionsEdit(MapCatalogPositionsEntity entity, DataTable Companies, DataTable NominalClass)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.MapCatalogPositionsEdit", new SqlParameter[] {
                    new SqlParameter("@MapCatalogPositionsId",entity.MapCatalogPositionsId),
                    new SqlParameter("@CategoryFTEId",entity.CategoryFTEId),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@PositionCode",entity.PositionCode),
                    new SqlParameter("@Companies",Companies),
                    new SqlParameter("@NominalClass",NominalClass),
                    new SqlParameter("@PaymentRateCode",entity.PaymentRateCode),
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
        /// Desactive or Active the map catalog positions
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbaEntity MapCatalogPositionsDesactiveteOrActive(MapCatalogPositionsEntity entity )
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.MapCatalogPositionsDesactiveteOrActive", new SqlParameter[] {
                    new SqlParameter("@MapCatalogPositionsId", entity.MapCatalogPositionsId),             
                    new SqlParameter("@Delete", entity.Deleted),
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
        /// List items by geografic code
        /// </summary>
        /// <param name="Geografia"></param>
        /// <returns></returns>
        public ListItem[] CompaniesListEnable(string Geografia)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CompaniesListEnable", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionID",Geografia)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("CompanyID").ToString(),
                    Text = r.Field<string>("CompanyName")
                }).ToArray();

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
        /// List items by geografic code
        /// </summary>
        /// <param name="Geografia"></param>
        /// <returns></returns>
        public ListItem[] NominalClassListEnabled(string Geografia)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.NominalClassListEnabled", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionID",Geografia)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("NominalClassId").ToString(),
                    Text = r.Field<string>("NominalClassName")
                }).ToArray();

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
        /// List items by geografic code
        /// </summary>
        /// <returns></returns>
        public ListItem[] CategoryFTEListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CategoryFTEListEnabled").Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("CategoryFTEId").ToString(),
                    Text = r.Field<string>("CategoryFTEDescripcion")
                }).ToArray();

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
        /// List items by geografic code
        /// </summary>
        /// <param name="Geografia"></param>
        /// <returns></returns>
        public ListItem[] PaymentRatesListByGeographicDivision(string Geografia)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PaymentRatesListByGeographicDivision", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode",Geografia)
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<short>("PaymentRateCode").ToString(),
                    Text = r.Field<string>("FunctionName")
                }).ToArray();

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
