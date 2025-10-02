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
    public class DiseaseCarePlacesDal : IDiseaseCarePlacesDal<DiseaseCarePlaceEntity>
    {
        /// <summary>
        /// List the Disease Care Places enabled
        /// </summary>
        /// <returns>The Disease Care Places</returns>
        public List<DiseaseCarePlaceEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DiseaseCarePlacesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DiseaseCarePlaceEntity
                {
                    DiseaseCarePlaceCode = r.Field<byte>("DiseaseCarePlaceCode"),
                    DiseaseCarePlaceDescriptionSpanish = r.Field<string>("DiseaseCarePlaceDescriptionSpanish"),
                    DiseaseCarePlaceDescriptionEnglish = r.Field<string>("DiseaseCarePlaceDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjDiseaseCarePlaces), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionDiseaseCarePlacesList, ex);
                }
            }
        }
    }
}