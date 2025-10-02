using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using System;
using System.Configuration;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace DOLE.HRIS.Application.DataAccess
{
    public static class Dal
    {
        /// <summary>
        /// Method to do query to database y return array of data rows
        /// </summary>
        /// <param name="procedure">store procedure name to execute in data base </param>
        /// <param name="sqlParams">sql params of store procedure</param>
        /// <param name="Timeout">time out</param>
        /// <returns></returns>
        public static DataRow[] Select(string procedure, SqlParameter[] sqlParams = null, int Timeout = 30)
        {
            DataRow[] result;
            SqlConnection SqlConnectionGlobal = new SqlConnection();

            try
            {
                SqlConnectionGlobal = new SqlConnection(ConnectionStringProvider.HRISConnectionString);
                var SqlCommandGlobal = new SqlCommand
                {
                    CommandText = procedure.Trim().ToString(),
                    CommandTimeout = Timeout,
                    CommandType = CommandType.StoredProcedure,
                    Connection = SqlConnectionGlobal
                };

                if (sqlParams != null)
                {
                    SqlCommandGlobal.Parameters.AddRange(sqlParams);
                }

                using (SqlCommandGlobal)
                {
                    SqlConnectionGlobal.Open();
                    var sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = SqlCommandGlobal
                    };

                    var dataSet = new DataTable();
                    sqlDataAdapter.Fill(dataSet);

                    result = dataSet.Select();
                }

                SqlConnectionGlobal.Close();
            }

            catch (SqlException sqlex)
            {
                throw new Exception(sqlex.Message);//DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (SqlConnectionGlobal.State == ConnectionState.Open) SqlConnectionGlobal.Close();

                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjGeneralParametersList, ex);
                }
            }

            return result;
        }

        /// <summary>
        /// Method to do query to database y return Data Set
        /// </summary>
        /// <param name="procedure"> store procedure name to execute in data base </param>
        /// <returns> Object DataSet </returns>
        public static DataSet QueryDataSet(string procedure, SqlParameter[] sqlParams = null, int Timeout = 30)
        {
            var dataSet = new DataSet();
            SqlConnection SqlConnectionGlobal = new SqlConnection();

            try
            {
                SqlConnectionGlobal = new SqlConnection(ConnectionStringProvider.HRISConnectionString);
                var SqlCommandGlobal = new SqlCommand
                {
                    CommandText = procedure.Trim().ToString(),
                    CommandType = CommandType.StoredProcedure,
                    Connection = SqlConnectionGlobal,
                    CommandTimeout = Timeout
                };

                if (sqlParams != null)
                {
                    SqlCommandGlobal.Parameters.AddRange(sqlParams);
                }

                using (SqlCommandGlobal)
                {
                    SqlConnectionGlobal.Open();
                    var sqlDataAdapter = new SqlDataAdapter
                    {
                        SelectCommand = SqlCommandGlobal
                    };

                    sqlDataAdapter.Fill(dataSet);
                }

                SqlConnectionGlobal.Close();
            }

            catch (Exception)
            {
                if (SqlConnectionGlobal.State == ConnectionState.Open) SqlConnectionGlobal.Close();
                throw;
            }

            return dataSet;
        }

        /// <summary>
        /// Method to do excute transaction scalar and return tuple
        /// </summary>
        /// <param name="procedure">store procedure name to execute in data base </param>
        /// <param name="sqlParams">sql params of store procedure</param>
        /// <param name="Timeout">time out</param>
        /// <returns></returns>
        public static Tuple<int, string> TransactionScalarTuple(string procedure, SqlParameter[] sqlParams = null, int Timeout = 30)
        {
            Tuple<int, string> tuple = null;
            SqlConnection SqlConnectionGlobal = new SqlConnection();

            try
            {
                SqlConnectionGlobal = new SqlConnection(ConnectionStringProvider.HRISConnectionString);
                var SqlCommandGlobal = new SqlCommand
                {
                    CommandText = procedure.Trim().ToString(),
                    CommandTimeout = Timeout,
                    CommandType = CommandType.StoredProcedure,
                    Connection = SqlConnectionGlobal,
                };

                if (sqlParams != null)
                {
                    SqlCommandGlobal.Parameters.AddRange(sqlParams);
                }

                using (SqlCommandGlobal)
                {
                    SqlConnectionGlobal.Open();
                    using (var reader = SqlCommandGlobal.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            tuple = Tuple.Create(reader.GetInt32(0), reader.GetString(1).ToString());
                        }
                    }
                }

                SqlConnectionGlobal.Close();
            }

            catch (Exception)
            {
                if (SqlConnectionGlobal.State == ConnectionState.Open) SqlConnectionGlobal.Close();
                throw;
            }

            return tuple;
        }

        /// <summary>
        /// Method to do excute transaction scalar and return int 32
        /// </summary>
        /// <param name="procedure">store procedure name to execute in data base </param>
        /// <param name="sqlParams">sql params of store procedure</param>
        /// <param name="Timeout">time out</param>
        /// <returns></returns>
        public static int TransactionScalar(string procedure, SqlParameter[] sqlParams = null, int Timeout = 30)
        {
            int result;
            SqlConnection SqlConnectionGlobal = new SqlConnection();

            try
            {
                SqlConnectionGlobal = new SqlConnection(ConnectionStringProvider.HRISConnectionString);
                var SqlCommandGlobal = new SqlCommand
                {
                    CommandText = procedure.Trim().ToString(),
                    CommandTimeout = Timeout,
                    CommandType = CommandType.StoredProcedure,
                    Connection = SqlConnectionGlobal,
                };

                if (sqlParams != null)
                {
                    SqlCommandGlobal.Parameters.AddRange(sqlParams);
                }

                using (SqlCommandGlobal)
                {
                    SqlConnectionGlobal.Open();
                    result = Convert.ToInt16(SqlCommandGlobal.ExecuteScalar());
                }

                SqlConnectionGlobal.Close();
            }

            catch (Exception)
            {
                if (SqlConnectionGlobal.State == ConnectionState.Open) SqlConnectionGlobal.Close();
                throw;
            }

            return result;
        }

        /// <summary>
        /// Method to do excute transaction scalar and return int 64
        /// </summary>
        /// <param name="procedure">store procedure name to execute in data base </param>
        /// <param name="sqlParams">sql params of store procedure</param>
        /// <param name="Timeout">time out</param>
        /// <returns></returns>
        public static Int64 TransactionScalar64(string procedure, SqlParameter[] sqlParams = null, int Timeout = 30)
        {
            Int64 result;
            SqlConnection SqlConnectionGlobal = new SqlConnection();

            try
            {
                SqlConnectionGlobal = new SqlConnection(ConnectionStringProvider.HRISConnectionString);
                var SqlCommandGlobal = new SqlCommand
                {
                    CommandText = procedure.Trim().ToString(),
                    CommandTimeout = Timeout,
                    CommandType = CommandType.StoredProcedure,
                    Connection = SqlConnectionGlobal,
                };

                if (sqlParams != null)
                {
                    SqlCommandGlobal.Parameters.AddRange(sqlParams);
                }

                using (SqlCommandGlobal)
                {
                    SqlConnectionGlobal.Open();
                    result = Convert.ToInt64(SqlCommandGlobal.ExecuteScalar());
                }

                SqlConnectionGlobal.Close();
            }

            catch (Exception)
            {
                if (SqlConnectionGlobal.State == ConnectionState.Open) SqlConnectionGlobal.Close();
                throw;
            }

            return result;
        }
    }

    public static class ExtensionsNumber
    {
        public static int? ToIntNullDataReader(this object val)
        {
            return val == DBNull.Value ? (int?)null : Convert.ToInt32(val);
        }
    }
}
