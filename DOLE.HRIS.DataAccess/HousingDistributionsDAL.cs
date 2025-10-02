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
    public class HousingDistributionsDal : IHousingDistributionsDal<HousingDistributionEntity>
    {
        /// <summary>
        /// List the Housing Distributions enabled
        /// </summary>
        /// <returns>The Housing Distributions</returns>
        public List<HousingDistributionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingDistributionsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingDistributionEntity
                {
                    HousingDistributionCode = r.Field<byte>("HousingDistributionCode"),
                    HousingDistributionDescriptionSpanish = r.Field<string>("HousingDistributionDescriptionSpanish"),
                    HousingDistributionDescriptionEnglish = r.Field<string>("HousingDistributionDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjHousingDistributions), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionHousingDistributionsList, ex);
                }
            }
        }
    }
}