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
    public class OtherServicesDal : IOtherServicesDal<OtherServiceEntity>
    {
        /// <summary>
        /// List the Other Services enabled
        /// </summary>
        /// <returns>The Other Services</returns>
        public List<OtherServiceEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.OtherServicesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new OtherServiceEntity
                {
                    OtherServiceCode = r.Field<byte>("OtherServiceCode"),
                    OtherServiceDescriptionSpanish = r.Field<string>("OtherServiceDescriptionSpanish"),
                    OtherServiceDescriptionEnglish = r.Field<string>("OtherServiceDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjOtherServices), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionOtherServicesList, ex);
                }
            }
        }
    }
}