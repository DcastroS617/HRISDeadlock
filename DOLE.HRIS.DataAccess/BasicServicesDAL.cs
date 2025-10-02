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
    public class BasicServicesDal : IBasicServicesDal<BasicServiceEntity>
    {
        /// <summary>
        /// List the Basic Services enabled
        /// </summary>
        /// <returns>The Basic Services</returns>
        public List<BasicServiceEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.BasicServicesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new BasicServiceEntity
                {
                    BasicServiceCode = r.Field<byte>("BasicServiceCode"),
                    BasicServiceDescriptionSpanish = r.Field<string>("BasicServiceDescriptionSpanish"),
                    BasicServiceDescriptionEnglish = r.Field<string>("BasicServiceDescriptionEnglish"),
                }).ToList();

                return result;
            }
            
            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjBasicServices), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionBasicServicesList, ex);
                }
            }
        }
    }
}