using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business
{
    public class WorkingDayTypesBLL : IWorkingDayTypesBLL<WorkingDayTypesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IWorkingDayTypesDAL<WorkingDayTypesEntity> workingDayTypesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="workingDayTypesDAL">Data access object</param>
        public WorkingDayTypesBLL(IWorkingDayTypesDAL<WorkingDayTypesEntity> workingDayTypesDAL)
        {
            this.workingDayTypesDAL = workingDayTypesDAL;
        }

        /// <summary>
        /// Save the WorkingDayTypes Records
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param> 
        public bool AddWorkingDayTypes(WorkingDayTypesEntity workingDayTypes)
        {
            try
            {
                return workingDayTypesDAL.AddWorkingDayTypes(workingDayTypes);
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
        /// Delete a WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypeCode">WorkingDayTypeCode</param>
        public bool DeleteWorkingDayTypes(int workingDayTypeCode)
        {
            try
            {
                return workingDayTypesDAL.DeleteWorkingDayTypes(workingDayTypeCode);
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
        /// List the WorkingDayTypes
        /// </summary>        
        /// <param name="geographicDivisionCode">geographic Division Code</param>
        /// <param name="divisionCode">division Code/param>
        /// <param name="workingDayTypeCode">working Day Type Code</param>
        /// <param name="workingDayTypesName">working Day Types Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingDayTypesEntity List</return>
        public PageHelper<WorkingDayTypesEntity> GetWorkingDayTypesList(string geographicDivisionCode, int divisionCode, int workingDayTypeCode, string workingDayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<WorkingDayTypesEntity> response = new PageHelper<WorkingDayTypesEntity>();
            try
            {
                response = workingDayTypesDAL.GetWorkingDayTypesList(
                    geographicDivisionCode,
                    divisionCode,
                    workingDayTypeCode,
                    workingDayTypesName,
                    sortExpression,
                    sortDirection,
                    pageNumber,
                    null);
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
        /// Update the WorkingDayTypes
        /// </summary>
        /// <param name="workingDayTypes">WorkingDayTypes</param>
        public bool UpdateWorkingDayTypes(WorkingDayTypesEntity workingDayTypes)
        {
            try
            {
                return workingDayTypesDAL.UpdateWorkingDayTypes(workingDayTypes);
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
        /// Get WorkingDayTypes By WorkingDayTypeCode
        /// </summary>        
        /// <param name="workingDayTypeCode">Working Day Type Code</param> 
        /// <returns>WorkingDayTypesEntity</returns> 
        public WorkingDayTypesEntity WorkingDayTypesByWorkingDayTypeCode(int workingDayTypeCode)
        {
            try
            {
                return workingDayTypesDAL.WorkingDayTypesByWorkingDayTypeCode(workingDayTypeCode);
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
