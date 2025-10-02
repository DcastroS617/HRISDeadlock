using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;
using DOLE.HRIS.Application.DataAccess.Interfaces;

namespace DOLE.HRIS.Application.DataAccess
{
    public class UserCodeDAL : IUserCodeDAL<UserCodeEntity>
    {
        /// <summary>
        /// Save the UserCode
        /// </summary>
        /// <param name="userCode">UserCode</param> 
        public bool AddUserCode(UserCodeEntity userCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.UserCodeAdd", new SqlParameter[] {
                    new SqlParameter("@CodeRoleApproverID",userCode.CodeRoleApproverID),
                    new SqlParameter("@CreatedBy",userCode.CreatedBy)
                });
                return result >= 1;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Delete an UserCode
        /// </summary>
        /// <param name="userCodeID">UserCodeID</param>
        public bool DeleteUserCode(int userCodeID)
        {
            try
            {
                Dal.TransactionScalar("OverTime.UserCodeDelete", new SqlParameter[] {
                    new SqlParameter("@UserCodeID",userCodeID)
                });
                return true;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// List the UserCode
        /// </summary>        
        /// <returns>UserCode</returns>
        public PageHelper<UserCodeEntity> GetUserCodeList(int userCodeID, int codeRoleApproverID, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.UserCodeListByFilters", new SqlParameter[] {
                    new SqlParameter("@UserCodeID", userCodeID),
                    new SqlParameter("@CodeRoleApproverID", codeRoleApproverID),
                    new SqlParameter("@SortExpression", sortExpression),
                    new SqlParameter("@SortDirection", sortDirection),
                    new SqlParameter("@PageNumber", pageNumber),
                    new SqlParameter("@PageSizeValue", pageSize)
                }, 360);

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new UserCodeEntity
                {
                    UserCodeID = r.Field<int>("UserCodeID"),
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    RoleApprover = r.Field<string>("RoleApprover"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();
                return new PageHelper<UserCodeEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// Save the UserCode
        /// </summary>
        /// <param name="userCode">UserCode</param> 
        public bool UpdateUserCode(UserCodeEntity userCode)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.UserCodeUpdate", new SqlParameter[] {
                    new SqlParameter("@UserCodeID",userCode.UserCodeID),
                    new SqlParameter("@CodeRoleApproverID",userCode.CodeRoleApproverID),
                    new SqlParameter("@LastModifiedUser",userCode.LastModifiedUser)
                });
                return result >= 1;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeism), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeism, ex);
                }
            }
        }

        /// <summary>
        /// Get UserCode Record By UserCodeID
        /// </summary>        
        /// <param name="userCodeID">UserCodeID</param>
        public UserCodeEntity UserCodeByUserCodeID(int userCodeID)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.UserCodeByUserCodeID", new SqlParameter[] {
                    new SqlParameter("@UserCodeID", userCodeID)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new UserCodeEntity
                {
                    UserCodeID = r.Field<int>("UserCodeID"),
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).FirstOrDefault();

                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjTrainingCenters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }
    }
}
