using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class OverTimeStatusBLL : IOverTimeStatusBLL<OverTimeStatusEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOverTimeStatusDAL<OverTimeStatusEntity> overTimeStatusDAL;
        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overTimeStatusDAL">Data access object</param>
        public OverTimeStatusBLL(IOverTimeStatusDAL<OverTimeStatusEntity> overTimeStatusDAL)
        {
            this.overTimeStatusDAL = overTimeStatusDAL;
        }

        /// <summary>
        /// List the OverTimeStatus
        /// </summary>        
        /// <param name="overTimeStatusCode">overTime Status Code</param>
        /// <param name="overTimeStatusName">overTime Status Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The OverTimeStatusEntity List</returns>
        public PageHelper<OverTimeStatusEntity> GetOverTimeStatusList(int overTimeStatusCode, string overTimeStatusName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<OverTimeStatusEntity> response = new PageHelper<OverTimeStatusEntity>();
            try
            {
                response = overTimeStatusDAL.GetOverTimeStatusList(
                    overTimeStatusCode,
                    overTimeStatusName,
                    sortExpression,
                    sortDirection,
                    pageNumber,
                    pageSize);
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
        /// Get OverTime Status By OverTime Status Code
        /// </summary>        
        /// <param name="overTimeStatusCode">overTime Status Code</param>
        /// <returns>Over Time Status Entity</returns> 
        public OverTimeStatusEntity OverTimeStatusByOverTimeStatusCode(int overTimeStatusCode)
        {
            try
            {
                return overTimeStatusDAL.OverTimeStatusByOverTimeStatusCode(overTimeStatusCode);
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
        /// Save the Over Time Status Records
        /// </summary>
        /// <param name="overTimeStatus">Over Time Status</param>
        public bool AddOverTimeStatus(OverTimeStatusEntity overTimeStatus)
        {
            try
            {
                return overTimeStatusDAL.AddOverTimeStatus(overTimeStatus);
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
        /// Update the OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatus">OverTimeStatus</param>
        public bool UpdateOverTimeStatus(OverTimeStatusEntity overTimeStatus)
        {
            try
            {
                return overTimeStatusDAL.UpdateOverTimeStatus(overTimeStatus);
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
        /// Delete a OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatusCode">Over Time Status Code</param>
        public bool DeleteOverTimeStatus(int overTimeStatusCode)
        {
            try
            {
                return overTimeStatusDAL.DeleteOverTimeStatus(overTimeStatusCode);
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
        /// Get list the OverTime Status
        /// </summary>        
        /// <returns>The OverTimeStatusEntity List</returns>
        public List<OverTimeStatusEntity> GetOverTimeStatusList()
        {
            try
            {
                return overTimeStatusDAL.GetOverTimeStatusList();
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
