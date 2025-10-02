using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class WorkingTimeTypesBLL : IWorkingTimeTypesBLL<WorkingTimeTypesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IWorkingTimeTypesDAL<WorkingTimeTypesEntity> workingTimeTypesDAL;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="workingTimeTypesDAL">Data access object</param>
        public WorkingTimeTypesBLL(IWorkingTimeTypesDAL<WorkingTimeTypesEntity> workingTimeTypesDAL)
        {
            this.workingTimeTypesDAL = workingTimeTypesDAL;
        }

        /// <summary>
        /// Get List the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        /// <param name="workingTimeTypeName">working Time Type Name</param>
        /// <param name="totalWorkingTime">total Working Time</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingTimeTypesEntity List</return>
        public PageHelper<WorkingTimeTypesEntity> GetWorkingTimeTypesList(int divisionCode, int workingTimeTypeCode, string workingTimeTypeName, int totalWorkingTime, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<WorkingTimeTypesEntity> response = new PageHelper<WorkingTimeTypesEntity>();
            try
            {
                response = workingTimeTypesDAL.GetWorkingTimeTypesList(
                      divisionCode
                    , workingTimeTypeCode
                    , workingTimeTypeName
                    , totalWorkingTime
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
        /// Get Working Time Types By Working Time Type Code
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        /// <returns>WorkingTimeTypesEntity</returns>
        public WorkingTimeTypesEntity GetWorkingTimeTypesByWorkingTimeTypeCode(int workingTimeTypeCode)
        {
            try
            {
                return workingTimeTypesDAL.GetWorkingTimeTypesByWorkingTimeTypeCode(workingTimeTypeCode);
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
        /// Save the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypesEntity">Working Time Types Entity</param>
        public bool AddWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes)
        {
            try
            {
                return workingTimeTypesDAL.AddWorkingTimeTypes(workingTimeTypes);
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
        /// Update the Working Time Types
        /// </summary>
        /// <param name="workingTimeTypesEntity">Working Time Types Entity</param>
        public bool UpdateWorkingTimeTypes(WorkingTimeTypesEntity workingTimeTypes)
        {
            try
            {
                return workingTimeTypesDAL.UpdateWorkingTimeTypes(workingTimeTypes);
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
        /// Delete a Working Time Types
        /// </summary>
        /// <param name="workingTimeTypeCode">Working Time Type Code</param>
        public bool DeleteWorkingTimeTypes(int workingTimeTypeCode)
        {
            try
            {
                return workingTimeTypesDAL.DeleteWorkingTimeTypes(workingTimeTypeCode);
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
        /// Get Working Time Types List For Dropdown
        /// </summary>
        /// <returns>The WorkingTimeTypesEntity List</returns>
        public List<WorkingTimeTypesEntity> GetWorkingTimeTypesListForDropdown()
        {
            try
            {
                return workingTimeTypesDAL.GetWorkingTimeTypesListForDropdown();
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