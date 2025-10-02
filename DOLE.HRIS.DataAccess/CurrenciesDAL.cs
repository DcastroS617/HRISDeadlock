using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class CurrenciesDal : ICurrenciesDal<CurrencyEntity>
    {
        /// <summary>
        /// List the currencies enabled
        /// </summary>
        /// <returns>The currencies</returns>
        public List<CurrencyEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CurrenciesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new CurrencyEntity
                {
                    CurrencyCode = r.Field<string>("CurrencyCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    CurrencyNameSpanish = r.Field<string>("CurrencyNameSpanish"),
                    CurrencyNameEnglish = r.Field<string>("CurrencyNameEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCurrencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCurrenciesList, ex);
                }
            }
        }
    }
}