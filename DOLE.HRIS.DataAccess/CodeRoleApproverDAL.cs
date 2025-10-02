using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Application.DataAccess.ConnectionStringProvider;

namespace DOLE.HRIS.Application.DataAccess
{
    public class CodeRoleApproverDAL : ICodeRoleApproverDAL<CodeRoleApproverEntity>
    {
        /// <summary>
        /// List the CodeRole Approver
        /// </summary>        
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="roleApprover">role Approver</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The CodeRoleApproverEntity List</returns>
        public PageHelper<CodeRoleApproverEntity> GetCodeRoleApproverList(string geographicDivisionCode, int divisionCode, int codeRoleApproverID, string roleApprover, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.CodeRoleApproverListByFilters", new SqlParameter[] {
                    new SqlParameter("@CodeRoleApproverID", codeRoleApproverID),
                    new SqlParameter("@RoleApprover", roleApprover),
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

                var result = ds.Tables[1].AsEnumerable().Select(r => new CodeRoleApproverEntity
                {
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    RoleApprover = r.Field<string>("RoleApprover"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();

                return new PageHelper<CodeRoleApproverEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// Get Code Role Approver By Code RoleApprover ID
        /// </summary>        
        /// <param name="codeRoleApproverID">CodeRoleApproverID</param> 
        public CodeRoleApproverEntity GetCodeRoleApproverByCodeRoleApproverID(int codeRoleApproverID)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.CodeRoleApproverByCodeRoleApproverID", new SqlParameter[] {
                    new SqlParameter("@CodeRoleApproverID", codeRoleApproverID)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new CodeRoleApproverEntity
                {
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    RoleApprover = r.Field<string>("RoleApprover"),
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

        /// <summary>
        /// Validate if name exist
        /// </summary>        
        /// <param name="roleAprovver">roleAprovver</param> 
        public  CodeRoleApproverEntity ValidateByName(string roleAprovver)
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.ListByName", new SqlParameter[] {
                    new SqlParameter("@RoleAprovver", roleAprovver)
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new CodeRoleApproverEntity
                {
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    RoleApprover = r.Field<string>("RoleApprover")
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

        /// <summary>
        /// Save the CodeRoleApprover
        /// </summary>
        /// <param name="GeographicDivisionCode">Geographic Division Code</param>                
        /// <param name="DivisionCode">Division Code</param>                
        /// <param name="CreatedBy">Created By</param>                
        /// <param name="codeRoleApprover">CodeRoleApprover</param>                
        /// <param name="LastModifiedUser">Last Modified User</param>             
        public bool AddCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.CodeRoleApproverAdd", new SqlParameter[] {
                    new SqlParameter("@RoleApprover",codeRoleApprover.RoleApprover),
                    new SqlParameter("@CreatedBy",codeRoleApprover.CreatedBy),
                    new SqlParameter("@LastModifiedUser",codeRoleApprover.CreatedBy)
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
        /// Update the CodeRoleApprover
        /// </summary>
        /// <param name="codeRoleApprover">CodeRoleApprover</param>            
        public bool UpdateCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.CodeRoleApproverUpdate", new SqlParameter[] {
                    new SqlParameter("@CodeRoleApproverID",codeRoleApprover.CodeRoleApproverID),
                    new SqlParameter("@RoleApprover",codeRoleApprover.RoleApprover),
                    new SqlParameter("@LastModifiedUser",codeRoleApprover.LastModifiedUser)
                });
                return result == 0;
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
        /// Delete a Code Role Approver
        /// </summary>
        /// <param name="codeRoleApproverID">CodeRoleApproverID</param>
        public bool DeleteCodeRoleApprover(int codeRoleApproverID)
        {
            try
            {
                var result = Dal.TransactionScalar("OverTime.CodeRoleApproverDelete", new SqlParameter[] {
                    new SqlParameter("@CodeRoleApproverID",codeRoleApproverID)
                });
                return result == 0;
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
        /// Get Code Role Approver For Dropdown
        /// </summary>        
        /// <returns>The CodeRoleApproverEntity list</returns>
        public List<CodeRoleApproverEntity> GetCodeRoleApproverListForDropdown()
        {
            try
            {
                var ds = Dal.QueryDataSet("OverTime.CodeRoleApproverListForDropdown", null, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new CodeRoleApproverEntity
                {
                    CodeRoleApproverID = r.Field<int>("CodeRoleApproverID"),
                    RoleApprover = r.Field<string>("RoleApprover"),
                    CreatedBy = r.Field<string>("CreatedBy"),
                    CreatedDate = r.Field<DateTime>("CreatedDate"),
                    LastModifiedUser = r.Field<string>("LastModifiedUser"),
                    LastModifiedDate = r.Field<DateTime>("LastModifiedDate"),
                }).ToList();
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
