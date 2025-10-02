using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class IndicatorsDal : IIndicatorsDAL<IndicatorEntity>
    {
        /// <summary>
        /// List all Indicators
        /// </summary>
        /// <returns>List of Indicator Entities</returns>
        public List<IndicatorEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("SocialResponsability.IndicatorsList", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new IndicatorEntity
                {
                    IndicatorCode = r.Field<int>("IndicatorCode"),
                    IndicatorName = r.Field<string>("IndicatorName")
                }).ToList();

                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}
