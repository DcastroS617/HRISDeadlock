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
    public class DiseasesDal : IDiseasesDal<DiseaseEntity>
    {
        /// <summary>
        /// List the Diseases enabled
        /// </summary>
        /// <returns>The Diseases</returns>
        public List<DiseaseEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DiseasesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DiseaseEntity
                {
                    DiseaseCode = r.Field<byte>("DiseaseCode"),
                    DiseaseDescriptionSpanish = r.Field<string>("DiseaseDescriptionSpanish"),
                    DiseaseDescriptionEnglish = r.Field<string>("DiseaseDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjDiseases), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionDiseasesList, ex);
                }
            }
        }
    }
}