using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GeneralParametersDal : IGeneralParametersDal<GeneralParameterEntity>
    {
        /// <summary>
        /// List an specific general parameter for a division and a module
        /// </summary>
        /// <param name="entity">Entity to filter, the division, module and general parameter name</param>
        /// <returns>The general parameter information</returns>
        public string  ListByFilter(string ParameterName)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.GeneralParametersGetValue", new SqlParameter[] {
                    new SqlParameter("@ParameterName", ParameterName)
                });

                var result = ds.Tables[0].Rows[0]["ParameterValue"].ToString();

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
