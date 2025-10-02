using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class RolesByDepartmentEmployeeBLL : IRolesByDepartmentEmployeeBLL<RolesByDepartmentEmployeeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IRolesByDepartmentEmployeeDAL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeBLL">Data access object</param>
        public RolesByDepartmentEmployeeBLL(IRolesByDepartmentEmployeeDAL<RolesByDepartmentEmployeeEntity> rolesByDepartmentEmployeeBLL)
        {
            this.rolesByDepartmentEmployeeBLL = rolesByDepartmentEmployeeBLL;
        }

        /// <summary>
        /// Get List the Roles By Department Employee
        /// </summary>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The RolesByDepartmentEmployeeEntity List</return>
        public PageHelper<RolesByDepartmentEmployeeEntity> GetRolesByDepartmentEmployeeList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<RolesByDepartmentEmployeeEntity> response;
            try
            {
                response = rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(
                      geographicDivisionCode 
                    , divisionCode
                    , departmentCode
                    , employeeCode
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSize);
                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Get Roles By Department Employee By Id
        /// </summary>        
        /// <param name="rolesByDepartmentEmployeeID">roles By Department Employee ID</param>
        /// <returns>RolesByDepartmentEmployeeEntity</returns>
        public RolesByDepartmentEmployeeEntity GetRolesByDepartmentEmployeeById(int rolesByDepartmentEmployeeID)
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeById(rolesByDepartmentEmployeeID);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Save the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param>
        public bool AddRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee)
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.AddRolesByDepartmentEmployee(rolesByDepartmentEmployee);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Update the Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeEntity">roles By Department Employee Entity</param>
        public bool UpdateRolesByDepartmentEmployee(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee)
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.UpdateRolesByDepartmentEmployee(rolesByDepartmentEmployee);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Delete an Roles By Department Employee
        /// </summary>
        /// <param name="rolesByDepartmentEmployeeID">roles By Department Employee ID</param>
        public bool DeleteRolesByDepartmentEmployee(int rolesByDepartmentEmployeeID)
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.DeleteRolesByDepartmentEmployee(rolesByDepartmentEmployeeID);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get Role Approvers List For Dropdown
        /// </summary>        
        /// <returns>The RoleApproversEntity List</returns>
        public List<RoleApproversEntity> GetRoleApproversListForDropdown()
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.GetRoleApproversListForDropdown();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get Roles By Department Employee By Criteria
        /// </summary>        
        /// <param name="criteria">criteria</param>
        /// <returns>The RolesByDepartmentEmployeeEntity List</returns>
        public List<RolesByDepartmentEmployeeEntity> GetRolesByDepartmentEmployeeList(RolesByDepartmentEmployeeEntity rolesByDepartmentEmployee)
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.GetRolesByDepartmentEmployeeList(rolesByDepartmentEmployee);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get Department List For Dropdown
        /// </summary>        
        /// <returns>The Departments List</returns>
        public List<Departments> GetDepartmentListForDropdown()
        {
            try
            {
                return rolesByDepartmentEmployeeBLL.GetDepartmentListForDropdown();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
    }
}
