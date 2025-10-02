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
    public class HousingTenuresDal : IHousingTenuresDal<HousingTenureEntity>
    {
        /// <summary>
        /// List the Housing Tenures enabled
        /// </summary>
        /// <returns>The Housing Tenures</returns>
        public List<HousingTenureEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingTenuresListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingTenureEntity
                {
                    HousingTenureCode = r.Field<byte>("HousingTenureCode"),
                    HousingTenureDescriptionSpanish = r.Field<string>("HousingTenureDescriptionSpanish"),
                    HousingTenureDescriptionEnglish = r.Field<string>("HousingTenureDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjHousingTenures), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionHousingTenuresList, ex);
                }
            }
        }
    }
}