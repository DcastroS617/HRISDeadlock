using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class UsersDal : IUsersDal<UserEntity>
    {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="entity">The user information</param>
        public void Add(UserEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.UsersAdd", new SqlParameter[] {
                    new SqlParameter("@ActiveDirectoryUserAccount",entity.ActiveDirectoryUserAccount),
                    new SqlParameter("@UserName",entity.UserName),
                    new SqlParameter("@EmailAddress",entity.EmailAddress),
                    new SqlParameter("@IsActive",entity.IsActive),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {                
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Count the total registered users
        /// </summary>
        /// <returns>Total users</returns>
        public int Count()
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.UsersCount");
                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersCount, ex);
                }
            }
        }

        /// <summary>
        /// Update the user information
        /// </summary>
        /// <param name="entity">The user information</param>
        public void Edit(UserEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.UsersEdit", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@UserName",entity.UserName),
                    new SqlParameter("@EmailAddress",entity.EmailAddress),
                    new SqlParameter("@IsActive",entity.IsActive),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersEdit, ex);
                }
            }
        }

        /// <summary>
        /// delete the user information
        /// </summary>
        /// <param name="entity">The user information</param>
        public DbaEntity DeleteUser(UserEntity entity)
        {
            try
            {
                var result = Dal.TransactionScalarTuple("HRIS.UserDelete", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                });

                return new DbaEntity { ErrorNumber=result.Item1,ErrorMessage=result.Item2 };
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersEdit, ex);
                }
            }
        }

        /// <summary>
        /// List all the active users
        /// </summary>
        /// <returns>A list of users</returns>
        public List<UserEntity> ListActive()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.UsersListActive");

                var result = ds.Tables[0].AsEnumerable().Select(r => new UserEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    UserName = r.Field<string>("UserName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    IsActive = r.Field<bool>("IsActive"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUsers), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the active directory account
        /// </summary>
        /// <param name="entity">The entity to filter, active directory account</param>
        /// <returns>The user</returns>
        public UserEntity ListByActiveDirectoryAccount(UserEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.UsersListByActiveDirectoryAccountV2", new SqlParameter[] {
                    new SqlParameter("@ActiveDirectoryUserAccount",entity.EmailAddress),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new UserEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    UserName = r.Field<string>("UserName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    IsActive = r.Field<bool>("IsActive"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListByActiveDirectoryAccount, ex);
                }
            }
        }

        /// <summary>
        /// List the user by the given filters and page config
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The users</returns>
        public PageHelper<UserEntity> ListByFilters(int divisionCode, string userName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.UsersListByFilters", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", divisionCode),
                    new SqlParameter("@UserName", userName),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSize", pageSize),
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new UserEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    UserName = r.Field<string>("UserName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    IsActive = r.Field<bool>("IsActive"),
                }).ToList();

                return new PageHelper<UserEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user for a grid page
        /// </summary>
        /// <param name="pageNumber">The grid page number</param>
        /// <returns>The users</returns>
        public List<UserEntity> ListByPage(int pageNumber)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.UsersListByPage", new SqlParameter[] {
                    new SqlParameter("@PageNumber",pageNumber),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new UserEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    UserName = r.Field<string>("UserName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    IsActive = r.Field<bool>("IsActive"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUsers), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the user code
        /// </summary>
        /// <param name="entity">The entity to filter, the user code</param>
        /// <returns>The user</returns>
        public UserEntity ListByUserCode(UserEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.UsersListByUserCode", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new UserEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    ActiveDirectoryUserAccount = r.Field<string>("ActiveDirectoryUserAccount"),
                    UserName = r.Field<string>("UserName"),
                    EmailAddress = r.Field<string>("EmailAddress"),
                    IsActive = r.Field<bool>("IsActive"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListByUserCode, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the division
        /// </summary>
        /// <param name="entity">The entity to filter, the user code</param>
        /// <returns>The user</returns>
        public List<UserEntity> ListByDivisionCode(int divisionCode)
        {
            List<UserEntity> users = new List<UserEntity>();

            try
            {
                using (SqlConnection connection = new SqlConnection(HRISConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("HRIS.UsersByDivision", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DivisionCode", SqlDbType.Int).Value = divisionCode;
                        connection.Open();
                        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                        while (dataReader.Read())
                        {
                            users.Add(new UserEntity(Convert.ToInt16(dataReader["UserCode"])
                                    , ""
                                    , Convert.ToString(dataReader["UserName"])
                                    , Convert.ToString(dataReader["EmailAddress"])
                                    , Convert.ToBoolean(dataReader["IsActive"])));
                        }

                        if (!dataReader.IsClosed)
                        {
                            dataReader.Close();
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjUser), sqlex);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjExceptionUsersListByUserCode, ex);
                }
            }

            return users;
        }
    }
}