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
    public class DiseaseFrequenciesDal : IDiseaseFrequenciesDal<DiseaseFrequencyEntity>
    {
        /// <summary>
        /// List the Disease Frequencies enabled
        /// </summary>
        /// <returns>The Disease Frequencies</returns>
        public List<DiseaseFrequencyEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DiseaseFrequenciesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DiseaseFrequencyEntity
                {
                    DiseaseFrequencyCode = r.Field<byte>("DiseaseFrequencyCode"),
                    DiseaseFrequencyDescriptionSpanish = r.Field<string>("DiseaseFrequencyDescriptionSpanish"),
                    DiseaseFrequencyDescriptionEnglish = r.Field<string>("DiseaseFrequencyDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjDiseaseFrequencies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionDiseaseFrequenciesList, ex);
                }
            }
        }
    }
}