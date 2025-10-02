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
    public class WaterSuppliesDal : IWaterSuppliesDal<WaterSupplyEntity>
    {
        /// <summary>
        /// List the Water Supplies enabled
        /// </summary>
        /// <returns>The Water Supplies</returns>
        public List<WaterSupplyEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.WaterSuppliesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new WaterSupplyEntity
                {
                    WaterSupplyCode = r.Field<byte>("WaterSupplyCode"),
                    WaterSupplyDescriptionSpanish = r.Field<string>("WaterSupplyDescriptionSpanish"),
                    WaterSupplyDescriptionEnglish = r.Field<string>("WaterSupplyDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjWaterSupplies), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionWaterSuppliesList, ex);
                }
            }
        }
    }
}