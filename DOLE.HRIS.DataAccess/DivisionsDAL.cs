using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    /// <summary>
    /// Data access class to handle operations on the Divisions table
    /// </summary>
    public class DivisionsDal : IDivisionsDal<DivisionEntity>
    {
        /// <summary>
        /// List divisions defined
        /// </summary>
        /// <returns>A list of Division entity</returns>
        public List<DivisionEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.DivisionsListAll");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DivisionEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CountryID = r.Field<string>("CountryID"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Shared.ErrorMessages.msjDataAccessExceptionListDivisionsAll, ex);
                }
            }
        }

        /// <summary>
        /// List divisions defined
        /// </summary>
        /// <returns>A list of Division entity</returns>
        public List<KeyValuePair<DivisionEntity, string>> ListAllWithGeographicDivision()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.DivisionsListAllWithGeographicDivision");

                var result = ds.Tables[0].AsEnumerable().Select(r => new KeyValuePair<DivisionEntity, string>(
                    new DivisionEntity(r.Field<int>("DivisionCode"), r.Field<string>("DivisionName")),
                    r.Field<string>("GeographicDivisionCode"))
                ).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Shared.ErrorMessages.msjDataAccessExceptionListDivisionsAll, ex);
                }
            }
        }

        /// <summary>
        /// List divisions defined by Active employess
        /// </summary>
        /// <returns>A list of Division entity</returns>
        public List<DivisionByActiveEmployeesEntity> ListAllDivisionByActiveEmployee()
        {
            try
            {
                var ds = Dal.QueryDataSet("gti.GetActiveDivisionesEmployess");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DivisionByActiveEmployeesEntity
                {
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Shared.ErrorMessages.msjDataAccessExceptionListDivisionsAll, ex);
                }
            }
        }
    }
}
