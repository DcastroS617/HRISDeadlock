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
    public class HousingAcquisitionWaysDal : IHousingAcquisitionWaysDal<HousingAcquisitionWayEntity>
    {
        /// <summary>
        /// List the Housing Acquisition Ways enabled
        /// </summary>
        /// <returns>The Housing Acquisition Ways</returns>
        public List<HousingAcquisitionWayEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.HousingAcquisitionWaysListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new HousingAcquisitionWayEntity
                {
                    HousingAcquisitionWayCode = r.Field<byte>("HousingAcquisitionWayCode"),
                    HousingAcquisitionWayDescriptionSpanish = r.Field<string>("HousingAcquisitionWayDescriptionSpanish"),
                    HousingAcquisitionWayDescriptionEnglish = r.Field<string>("HousingAcquisitionWayDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjHousingAcquisitionWays), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionHousingAcquisitionWaysList, ex);
                }
            }
        }
    }
}