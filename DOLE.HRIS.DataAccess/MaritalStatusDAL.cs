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
    public class MaritalStatusDal : IMaritalStatusDal<MaritalStatusEntity>
    {
        /// <summary>
        /// List the marital status enabled
        /// </summary>
        /// <returns>The marital status</returns>
        public List<MaritalStatusEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.MaritalStatusListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new MaritalStatusEntity
                {
                    MaritalStatusCode = r.Field<byte>("MaritalStatusCode"),
                    MaritalStatusDescriptionSpanish = r.Field<string>("MaritalStatusDescriptionSpanish"),
                    MaritalStatusDescriptionEnglish = r.Field<string>("MaritalStatusDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjMaritalStatus), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionMaritalStatusList, ex);
                }
            }
        }
    }
}