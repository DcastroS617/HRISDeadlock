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
    public class PoliticalDivisionsLabelsDal : IPoliticalDivisionsLabelsDal<PoliticalDivisionLabelEntity>
    {
        /// <summary>
        /// List the political division labels enabled
        /// </summary>
        /// <returns>The political division labels</returns>
        public List<PoliticalDivisionLabelEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PoliticalDivisionsLabelsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PoliticalDivisionLabelEntity
                {
                    CountryID = r.Field<string>("CountryID"),
                    PoliticalDivisionLevel = r.Field<byte>("PoliticalDivisionLevel"),
                    PoliticalDivisionLabelTextSpanish = r.Field<string>("PoliticalDivisionLabelTextSpanish"),
                    PoliticalDivisionLabelTextEnglish = r.Field<string>("PoliticalDivisionLabelTextEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPoliticalDivisionsLabels), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPoliticalDivisionsLabelsList, ex);
                }
            }
        }
    }
}