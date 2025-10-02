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
    public class ChronicDiseasesDal : IChronicDiseasesDAL<ChronicDiseasesEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<ChronicDiseasesEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ChronicDiseasesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ChronicDiseasesEntity
                {
                    ChronicDiseaseCode = r.Field<byte>("ChronicDiseaseCode"),
                    ChronicDiseaseDescriptionSpanish = r.Field<string>("ChronicDiseaseDescriptionSpanish"),
                    ChronicDiseaseDescriptionEnglish = r.Field<string>("ChronicDiseaseDescriptionEnglish")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msgChronicDiseasesList), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgChronicDiseasesList, ex);
                }
            }

        }
    }
}
