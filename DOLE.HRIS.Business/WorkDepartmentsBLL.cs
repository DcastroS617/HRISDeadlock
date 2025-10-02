using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business
{
    public class WorkDepartmentsBLL : IWorkDepartmentsBLL<WorkDepartmentsEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IWorkDepartmentsDAL<WorkDepartmentsEntity> workDepartmentsBLL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="workDepartmentsBLL">Data access object</param>
        public WorkDepartmentsBLL(IWorkDepartmentsDAL<WorkDepartmentsEntity> workDepartmentsBLL)
        {
            this.workDepartmentsBLL = workDepartmentsBLL;
        }

        /// <summary>
        /// Get list the WorkDepartments
        /// </summary>   
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code</param>
        /// <param name="userCodeID">user Code ID</param>
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The WorkDepartmentsEntity List</returns>
        public PageHelper<WorkDepartmentsEntity> GetWorkDepartmentsList(string geographicDivisionCode, int divisionCode, int workDepartmentCode, int overtimeCompanieCode, string workDepartmentName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<WorkDepartmentsEntity> response = new PageHelper<WorkDepartmentsEntity>();
            try
            {
                response = workDepartmentsBLL.GetWorkDepartmentsList(
                      geographicDivisionCode 
                    , divisionCode
                    , workDepartmentCode
                    , overtimeCompanieCode
                    , workDepartmentName
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
        /// Get Work Departments By Work Department Code
        /// </summary>        
        /// <param name="workDepartmentCode">Work Department Code</param> 
        public WorkDepartmentsEntity GetWorkDepartmentsByWorkDepartmentCode(int workDepartmentCode)
        {
            try
            {
                return workDepartmentsBLL.GetWorkDepartmentsByWorkDepartmentCode(workDepartmentCode);
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
        /// Save the Work Departments
        /// </summary>
        /// <param name="workDepartmentsEntity">work Departments Entity</param>  
        public bool AddWorkDepartments(WorkDepartmentsEntity workDepartments)
        {
            try
            {
                return workDepartmentsBLL.AddWorkDepartments(workDepartments);
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
        /// Update the Work Departments
        /// </summary>
        /// <param name="workDepartmentsEntity">work Departments Entity</param>
        public bool UpdateWorkDepartments(WorkDepartmentsEntity workDepartments)
        {
            try
            {
                return workDepartmentsBLL.UpdateWorkDepartments(workDepartments);
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
        /// Delete a Work Departments
        /// </summary>
        /// <param name="workDepartmentCode">Work Department Code</param>
        public bool DeleteWorkDepartments(int workDepartmentCode)
        {
            try
            {
                return workDepartmentsBLL.DeleteWorkDepartments(workDepartmentCode);
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
