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
    public class ServicesAvailabilityDal : IServicesAvailabilityDal<ServicesAvailabilityEntity>
    {
        /// <summary>
        /// List the Services Availability enabled
        /// </summary>
        /// <returns>The Services Availability</returns>
        public List<ServicesAvailabilityEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ServicesAvailabilityListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ServicesAvailabilityEntity
                {
                    ServicesAvailabilityCode = r.Field<byte>("ServicesAvailabilityCode"),
                    ServicesAvailabilityDescriptionSpanish = r.Field<string>("ServicesAvailabilityDescriptionSpanish"),
                    ServicesAvailabilityDescriptionEnglish = r.Field<string>("ServicesAvailabilityDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjServicesAvailability), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionServicesAvailabilityList, ex);
                }
            }
        }
    }
}