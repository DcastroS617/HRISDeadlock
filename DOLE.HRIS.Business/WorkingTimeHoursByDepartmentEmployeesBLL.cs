using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class WorkingTimeHoursByDepartmentEmployeesBLL : IWorkingTimeHoursByDepartmentEmployeesBLL<WorkingTimeHoursByDepartmentEmployeesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IWorkingTimeHoursByDepartmentEmployeesDAL<WorkingTimeHoursByDepartmentEmployeesEntity> workingTimeHoursByDepartmentEmployeesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesDAL">Data access object</param>
        public WorkingTimeHoursByDepartmentEmployeesBLL(IWorkingTimeHoursByDepartmentEmployeesDAL<WorkingTimeHoursByDepartmentEmployeesEntity> workingTimeHoursByDepartmentEmployeesDAL)
        {
            this.workingTimeHoursByDepartmentEmployeesDAL = workingTimeHoursByDepartmentEmployeesDAL;
        }

        /// <summary>
        /// Get List the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="departmentCode">department Code</param>
        /// <param name="employeeCode">employee Code</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeHoursByDepartmentEmployeesEntity List</return>
        public PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> GetWorkingTimeHoursByDepartmentEmployeesList(string geographicDivisionCode, int divisionCode, string departmentCode, string employeeCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity> response = new PageHelper<WorkingTimeHoursByDepartmentEmployeesEntity>();
            try
            {
                response = workingTimeHoursByDepartmentEmployeesDAL.GetWorkingTimeHoursByDepartmentEmployeesList(
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
        /// Get Working Time Hours By Department Employees By Id
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">working TimeHours By Department Employees ID</param>
        /// <returns>WorkingTimeHoursByDepartmentEmployeesEntity</returns>
        public WorkingTimeHoursByDepartmentEmployeesEntity GetWorkingTimeHoursByDepartmentEmployeesById(int workingTimeHoursByDepartmentEmployeesID)
        {
            try
            {
                return workingTimeHoursByDepartmentEmployeesDAL.GetWorkingTimeHoursByDepartmentEmployeesById(workingTimeHoursByDepartmentEmployeesID);
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
        /// Save the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesEntity">working TimeHours By Department Employees Entity</param>
        public bool AddWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees)
        {
            try
            {
                return workingTimeHoursByDepartmentEmployeesDAL.AddWorkingTimeHoursByDepartmentEmployees(workingTimeHoursByDepartmentEmployees);
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
        /// Update the Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesEntity">working TimeHours By Department Employees Entity</param>
        public bool UpdateWorkingTimeHoursByDepartmentEmployees(WorkingTimeHoursByDepartmentEmployeesEntity workingTimeHoursByDepartmentEmployees)
        {
            try
            {
                return workingTimeHoursByDepartmentEmployeesDAL.UpdateWorkingTimeHoursByDepartmentEmployees(workingTimeHoursByDepartmentEmployees);
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
        /// Delete a  Working Time Hours By Department Employees
        /// </summary>
        /// <param name="workingTimeHoursByDepartmentEmployeesID">working Time Hours By Department Employees ID</param>
        public bool DeleteWorkingTimeHoursByDepartmentEmployees(int workingTimeHoursByDepartmentEmployeesID)
        {
            try
            {
                return workingTimeHoursByDepartmentEmployeesDAL.DeleteWorkingTimeHoursByDepartmentEmployees(workingTimeHoursByDepartmentEmployeesID);
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
