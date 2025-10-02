using DOLE.HRIS.Exceptions;
using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DOLE.HRIS.Application.DataAccess
{
    /// <summary>
    /// Data access class to handle operations on the Security.Consecutives table
    /// </summary>
    public partial class ConsecutivesDal
    {
        /// <summary>
        /// Get next consecutive for especific table
        /// </summary>
        /// <param name="consecutiveName">Name of the consecutive to generate</param>
        /// <returns></returns>
        public T GetNextConsecutiveByName<T>(string consecutiveName) 
        {
            try
            {
                var result = Dal.TransactionScalar("Security.ConsecutivesGetNextValue", new SqlParameter[] {
                    new SqlParameter("@ConsecutiveName",consecutiveName),
                });

                return (T)Convert.ChangeType(result, typeof(T));
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, string.Empty), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Shared.ErrorMessages.msjDataAccessExceptionGetNextConsecutive, ex);
                }
            }
        }
    }
}
