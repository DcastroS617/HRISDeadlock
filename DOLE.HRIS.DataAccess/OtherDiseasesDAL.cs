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
    public class OtherDiseasesDal : IOtherDiseasesDal<OtherDiseaseEntity>
    {
        /// <summary>
        /// List the Other Diseases enabled
        /// </summary>
        /// <returns>The Other Diseases</returns>
        public List<OtherDiseaseEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.OtherDiseasesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new OtherDiseaseEntity
                {
                    OtherDiseaseCode = r.Field<byte>("OtherDiseaseCode"),
                    OtherDiseaseDescriptionSpanish = r.Field<string>("OtherDiseaseDescriptionSpanish"),
                    OtherDiseaseDescriptionEnglish = r.Field<string>("OtherDiseaseDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjOtherDiseases), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionOtherDiseasesList, ex);
                }
            }
        }
    }
}