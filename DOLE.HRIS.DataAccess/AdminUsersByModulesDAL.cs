using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
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
    public class AdminUsersByModulesDal : IAdminUsersByModulesDal<AdminUserByModuleEntity>
    {
        /// <summary>
        /// Check if the user is admin for the module in the division
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="moduleCode">Module code</param>
        /// <returns>True if the user is admin for the module in the division. False otherwise</returns>
        public bool IsUserAdmin(string username, int divisionCode, int moduleCode)
        {
            try
            {
                var result = Dal.TransactionScalar("HRIS.AdminUsersByModulesIsUserAdmin", new SqlParameter[] {
                    new SqlParameter("@UserName",username),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@ModuleCode",moduleCode),
                });

                return Convert.ToBoolean(result);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesActivate, ex);
                }
            }
        }
        
        /// <summary>
        /// Activate the permission to module for user
        /// </summary>
        /// <param name="entity">The user by division an module</param>
        public void Activate(AdminUserByModuleEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AdminUsersByModulesActivate", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ModuleCode",entity.ModuleCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesActivate, ex);
                }
            }
        }
       
        /// <summary>
        /// Add the permission to module for user
        /// </summary>
        /// <param name="entity">The user by division an module</param>
        public void Add(AdminUserByModuleEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AdminUsersByModulesAdd", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ModuleCode",entity.ModuleCode),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesAdd, ex);
                }
            }
        }
       
        /// <summary>
        /// Delete the permission to module for user
        /// </summary>
        /// <param name="entity">The user by division an module</param>
        public void Delete(AdminUserByModuleEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AdminUsersByModulesDelete", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ModuleCode",entity.ModuleCode),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesDelete, ex);
                }
            }
        }
       
        /// <summary>
        /// Edit the permission to module for user
        /// </summary>
        /// <param name="entity">The user by division an module</param>
        public void Edit(AdminUserByModuleEntity entity)
        {
            try
            {
                Dal.TransactionScalar("HRIS.AdminUsersByModulesEdit", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ModuleCode",entity.ModuleCode),
                    new SqlParameter("@SearchEnabled",entity.SearchEnabled),
                    new SqlParameter("@Deleted",entity.Deleted),
                    new SqlParameter("@LastModifiedUser",entity.LastModifiedUser),
                });
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesEdit, ex);
                }
            }
        }
        
        /// <summary>
        /// List the permission to module for user by the given filters
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="workingDivisionCode">The working division code</param>
        /// <param name="divisionCode">The Division code</param
        /// <param name="moduleCode">The module code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The divisions by module meeting the given filters and page config</returns>
        public PageHelper<AdminUserByModuleEntity> ListByFilters(short userCode, int workingDivisionCode, int? divisionCode, byte? moduleCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AdminUsersByModulesListByFilters", new SqlParameter[] {
                    new SqlParameter("@UserCode",userCode),
                    new SqlParameter("@WorkingDivisionCode",workingDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@ModuleCode",moduleCode),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                });
 
                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();

                var result = ds.Tables[1].AsEnumerable().Select(r => new AdminUserByModuleEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    ModuleCode = r.Field<byte>("ModuleCode"),
                    ModuleName = r.Field<string>("ModuleName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).ToList();

                return new PageHelper<AdminUserByModuleEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesListByFilters, ex);
                }
            }
        }
        
        /// <summary>
        /// List the permission of the user for the division and module
        /// </summary>
        /// <param name="T">The specifc permission by user code, division and module</param>
        /// <returns>The permission for the user by division an module</returns>
        public AdminUserByModuleEntity ListByUserDivisionModule(AdminUserByModuleEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.AdminUsersByModulesListByUserDivisionModule", new SqlParameter[] {
                    new SqlParameter("@UserCode",entity.UserCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@ModuleCode",entity.ModuleCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new AdminUserByModuleEntity
                {
                    UserCode = r.Field<short>("UserCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    ModuleCode = r.Field<byte>("ModuleCode"),
                    ModuleName = r.Field<string>("ModuleName"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                    Deleted = r.Field<bool>("Deleted"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAdminUsersByModules), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAdminUsersByModulesListByUserDivisionModule, ex);
                }
            }
        }
    }
}
