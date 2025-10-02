using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GtiPeriodParameterDivisionCurrencyDAL : IGtiPeriodParameterDivisionCurrencyDAL
    {
        /// <summary>
        /// Adds or updates a period parameter division currency in the database.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be added or updated</param>
        /// <returns>Returns the updated or added PeriodParameterDivisionCurrencyEntity</returns>
        public PeriodParameterDivisionCurrencyEntity AddOrUpdate(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity)
        {
            var ds = Dal.QueryDataSet("Gti.GtiPeriodParameterDivisionCurrencyAddorUpdate", new SqlParameter[] {
                    new SqlParameter("@PeriodParameterDivisionCurrencyId",periodParameterDivisionCurrencyEntity.PeriodParameterDivisionCurrencyId),
                    new SqlParameter("@PeriodParameterDivisionCurrencyName",periodParameterDivisionCurrencyEntity.PeriodParameterDivisionCurrencyName == "" ? DBNull.Value : (object)periodParameterDivisionCurrencyEntity.PeriodParameterDivisionCurrencyName),
                    new SqlParameter("@DivisionCode", periodParameterDivisionCurrencyEntity.DivisionCode == 0 ? DBNull.Value : (object)periodParameterDivisionCurrencyEntity.DivisionCode),
                    new SqlParameter("@GeographicDivisionID", periodParameterDivisionCurrencyEntity.GeographicDivisionID == "" ? DBNull.Value : (object)periodParameterDivisionCurrencyEntity.GeographicDivisionID),
                    new SqlParameter("@NominalClassId", periodParameterDivisionCurrencyEntity.NominalClassId == "" ? DBNull.Value : (object)periodParameterDivisionCurrencyEntity.NominalClassId),
                    new SqlParameter("@CurrencyCode",  periodParameterDivisionCurrencyEntity.CurrencyCode == "" ? DBNull.Value : (object)periodParameterDivisionCurrencyEntity.CurrencyCode),
                    new SqlParameter("@Deleted", periodParameterDivisionCurrencyEntity.Deleted),
                    new SqlParameter("@SearchEnabled", periodParameterDivisionCurrencyEntity.SearchEnabled)
                });

            var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodParameterDivisionCurrencyEntity
            {
                PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                PeriodParameterDivisionCurrencyName = r.Field<string>("PeriodParameterDivisionCurrencyName"),
                ErrorNumber = r.Field<int>("MsgCode"),
                ErrorMessage = r.Field<string>("MsgError")
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Deletes a period parameter division currency entity from the database.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be deleted</param>
        public void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity)
        {
            throw new NotImplementedException();
        }

        // <summary>
        /// Retrieves a period parameter division currency entity by its ID.
        /// </summary>
        /// <param name="PeriodParameterDivisionCurrencyId">The ID of the period parameter division currency</param>
        /// <returns>Returns the entity matching the provided ID</returns>
        public PeriodParameterDivisionCurrencyEntity ListByKey(int PeriodParameterDivisionCurrencyId)
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.GetPeriodParameterDivisionCurrencyByKey", new SqlParameter[] {
                    new SqlParameter("@PeriodParameterDivisionCurrencyId",PeriodParameterDivisionCurrencyId) });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodParameterDivisionCurrencyEntity
                {
                    PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                    PeriodParameterDivisionCurrencyName = r.Field<string>("PeriodParameterDivisionCurrencyName"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    GeographicDivisionID = r.Field<string>("GeographicDivisionID"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled")
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all active currencies from the database.
        /// </summary>
        /// <returns>Returns a list of active CurrencyEntity objects</returns>
        public List<CurrencyEntity> ListCurrenciesActive()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.DistinctCurrenciesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new CurrencyEntity
                {
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all enabled nominal classes.
        /// </summary>
        /// <returns>Returns a list of NominalClassEntity objects</returns>
        public List<NominalClassEntity> ListNominalClassEnable()
        {
            try
            {
                var ds = Dal.QueryDataSet("DOLE.NominalClassListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new NominalClassEntity
                {
                    NominalClassId = r.Field<string>("NominalClassId"),
                    NominalClassName = r.Field<string>("NominalClassName")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists enabled nominal classes filtered by geographic division code.
        /// </summary>
        /// <param name="GeographicDivisionCode">The code of the geographic division</param>
        /// <returns>Returns a list of NominalClassEntity objects matching the given division</returns>
        public List<NominalClassEntity> ListNominalClassEnableByDivision(string GeographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("DOLE.NominalClassListEnabledByDivisionForGTI", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", GeographicDivisionCode) });

                var result = ds.Tables[0].AsEnumerable().Select(r => new NominalClassEntity
                {
                    NominalClassId = r.Field<string>("NominalClassId"),
                    NominalClassName = r.Field<string>("NominalClassName")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all geographic divisions with their associated division codes.
        /// </summary>
        /// <returns>Returns a list of DivisionEntity objects</returns>
        public List<DivisionEntity> ListGeographicDivisionsByDivisions()
        {
            try
            {
                var ds = Dal.QueryDataSet("DOLE.DivisionsListAllWithGeographicDivision");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DivisionEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all name of geographic divisions with their associated division codes.
        /// </summary>
        /// <returns>Returns a list of DivisionEntity objects</returns>
        public List<PeriodParameterDivisionCurrencyEntity> ListNameGeographicDivisionsByDivisions()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.ListNameGeographicDivisionsByDivisions");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodParameterDivisionCurrencyEntity
                {
                    PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                    PeriodParameterDivisionCurrencyName = r.Field<string>("PeriodParameterDivisionCurrencyName")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists all period parameter division currency records.
        /// </summary>
        /// <returns>Returns a list of PeriodParameterDivisionCurrencyEntity objects</returns>
        public List<PeriodParameterDivisionCurrencyEntity> ListGtiPeriodParameterDivisionCurrency()
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.PeriodParameterDivisionCurrencyList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodParameterDivisionCurrencyEntity
                {
                    PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                    PeriodParameterDivisionCurrencyName = r.Field<string>("PeriodParameterDivisionCurrencyName"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    GeographicDivisionID = r.Field<string>("GeographicDivisionID"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled")
                }).ToList();

                return result;

            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
        /// Lists period parameter division currency records filtered by various parameters.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">Entity with filter values</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="sortDirection">Sort direction</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <returns>Returns a PageHelper object with the filtered results and pagination info</returns>
        public PageHelper<PeriodParameterDivisionCurrencyEntity> ListGtiPeriodParameterDivisionCurrencyByFilters(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity, string sortExpression, string sortDirection, int pageNumber, int pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("GTI.PeriodParameterDivisionCurrencyListByFilters", new SqlParameter[] {
                    new SqlParameter("@PeriodParameterDivisionCurrencyName", periodParameterDivisionCurrencyEntity.PeriodParameterDivisionCurrencyName),
                    new SqlParameter("@GeographicDivisionID", periodParameterDivisionCurrencyEntity.GeographicDivisionID),
                    new SqlParameter("@NominalClassId", periodParameterDivisionCurrencyEntity.NominalClassId),
                    new SqlParameter("@CurrencyCode", periodParameterDivisionCurrencyEntity.CurrencyCode),
                    new SqlParameter("@Deleted", periodParameterDivisionCurrencyEntity.Deleted),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize)
                }, 120);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new PeriodParameterDivisionCurrencyEntity
                {
                    PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                    PeriodParameterDivisionCurrencyName = r.Field<string>("PeriodParameterDivisionCurrencyName"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    GeographicDivisionID = r.Field<string>("GeographicDivisionID"),
                    NominalClassId = r.Field<string>("NominalClassId"),
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish"),
                    SearchEnabled = r.Field<bool>("SearchEnabled")
                }).ToList();

                return new PageHelper<PeriodParameterDivisionCurrencyEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGtiPeriodListbyFilter), sqlex);
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
