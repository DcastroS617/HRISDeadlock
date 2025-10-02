using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class LocalEnvironmentsByDivisionsDal : ILocalEnvironmentsByDivisionsDal
    {
        /// <summary>
        /// List the local environment configuration for a división
        /// </summary>
        /// <param name="entity">Entity to filter, division code</param>
        /// <returns>The local environment configuration</returns>
        public LocalEnvironmentByDivision ListByDivisionCode(LocalEnvironmentByDivision entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.LocalEnvironmentsByDivisionsListByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new LocalEnvironmentByDivision
                {
                    DivisionCode = r.Field<byte>("DivisionCode"),
                    ServiceBasePath = r.Field<string>("ServiceBasePath"),
                    DataBaseConnectionName = r.Field<string>("DataBaseConnectionName"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjLocalEnvironmentByDivision), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjLocalEnvironmentsByDivisionsListByDivision, ex);
                }
            }
        }
    }
}
