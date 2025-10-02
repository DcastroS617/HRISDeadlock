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
    public class LanguagesDal : ILanguagesDal<LanguageEntity>
    {
        /// <summary>
        /// List the languages enabled
        /// </summary>
        /// <returns>The languages</returns>
        public List<LanguageEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.LanguagesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new LanguageEntity
                {
                    LanguageCode = r.Field<byte>("LanguageCode"),
                    LanguageNameSpanish = r.Field<string>("LanguageNameSpanish"),
                    LanguageNameEnglish = r.Field<string>("LanguageNameEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjLanguages), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionLanguagesList, ex);
                }
            }
        }
    }
}