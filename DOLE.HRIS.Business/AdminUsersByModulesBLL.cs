using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class AdminUsersByModulesBll : IAdminUsersByModulesBll<AdminUserByModuleEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAdminUsersByModulesDal<AdminUserByModuleEntity> AdminUsersByModulesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AdminUsersByModulesBll(IAdminUsersByModulesDal<AdminUserByModuleEntity> objDal)
        {
            AdminUsersByModulesDal = objDal;
        }

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
                return AdminUsersByModulesDal.IsUserAdmin(username, divisionCode, moduleCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesActivate, ex);
                }
            }
        }

        /// <summary>
        /// Activate the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Activate(short userCode, int divisionCode, byte moduleCode, string lastModifiedUser)
        {
            try
            {                
                AdminUsersByModulesDal.Activate(new AdminUserByModuleEntity(userCode, divisionCode, moduleCode, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="searchEnabled">Search enabled ?</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public Tuple<bool, AdminUserByModuleEntity> Add(short userCode, int divisionCode, byte moduleCode, bool searchEnabled, string lastModifiedUser)
        {
            try
            {
                AdminUserByModuleEntity previousAdminUserByModule = AdminUsersByModulesDal.ListByUserDivisionModule(new AdminUserByModuleEntity(userCode, divisionCode, moduleCode));

                if (previousAdminUserByModule == null)
                {
                    AdminUserByModuleEntity entity = new AdminUserByModuleEntity(userCode, divisionCode, moduleCode, searchEnabled, lastModifiedUser);

                    AdminUsersByModulesDal.Add(entity);

                    return new Tuple<bool, AdminUserByModuleEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, AdminUserByModuleEntity>(false, previousAdminUserByModule);
                }               
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Delete(short userCode, int divisionCode, byte moduleCode, string lastModifiedUser)
        {
            try
            {
                AdminUsersByModulesDal.Delete(new AdminUserByModuleEntity(userCode, divisionCode, moduleCode, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the permission to module for user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <param name="searchEnabled">Search enabled ?</param>
        /// <param name="deleted">Deleted?</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Edit(short userCode, int divisionCode, byte moduleCode, bool searchEnabled, bool deleted, string lastModifiedUser)
        {
            try
            {
                AdminUsersByModulesDal.Edit(new AdminUserByModuleEntity(userCode, divisionCode, moduleCode, searchEnabled, deleted, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesEdit, ex);
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
        public PageHelper<AdminUserByModuleEntity> ListByFilters(short userCode, int workingDivisionCode, int? divisionCode, byte? moduleCode, string sortExpression, string sortDirection, int pageNumber)
        {            
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<AdminUserByModuleEntity> pageHelper = AdminUsersByModulesDal.ListByFilters(userCode
                    , workingDivisionCode
                    , divisionCode
                    , moduleCode
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the permission of the user for the division and module
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="moduleCode">The module code</param>
        /// <returns>The permission for the user by division an module</returns>
        public AdminUserByModuleEntity ListByUserDivisionModule(short userCode, int divisionCode, byte moduleCode)
        {
            AdminUserByModuleEntity response;
            try
            {
                response = AdminUsersByModulesDal.ListByUserDivisionModule(new AdminUserByModuleEntity(userCode, divisionCode, moduleCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjAdminUsersByModulesListByUserDivisionModule, ex);
                }
            }
            return response;
        }
    }
}