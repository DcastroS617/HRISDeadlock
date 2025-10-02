using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business
{
    public class OvertimeApprovalTypesBLL : IOvertimeApprovalTypesBLL<OvertimeApprovalTypesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOvertimeApprovalTypesDAL<OvertimeApprovalTypesEntity> overtimeApprovalTypesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overtimeApprovalTypesDAL">Data access object</param>
        public OvertimeApprovalTypesBLL(IOvertimeApprovalTypesDAL<OvertimeApprovalTypesEntity> overtimeApprovalTypesDAL)
        {
            this.overtimeApprovalTypesDAL = overtimeApprovalTypesDAL;
        }

        /// <summary>
        /// Save the Overtime Approval Types Records
        /// </summary>
        /// <param name="overtimeApprovalTypes">Overtime Approval Types</param> 
        public bool AddOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes)
        {
            try
            {
                return overtimeApprovalTypesDAL.AddOvertimeApprovalTypes(overtimeApprovalTypes);
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
        /// Delete a Overtime Approval Types
        /// </summary>
        /// <param name="overtimeApprovalTypeCode">Overtime Approval Types Code</param>
        public bool DeleteOvertimeApprovalTypes(int overtimeApprovalTypeCode)
        {
            try
            {
                return overtimeApprovalTypesDAL.DeleteOvertimeApprovalTypes(overtimeApprovalTypeCode);
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
        /// Get list the Overtime Approval Types
        /// </summary>        
        /// <param name="geographicDivisionCode">geographic Division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="overtimeApprovalTypeCode">overtime Approval Type Code</param>
        /// <param name="overtimeApprovalTypeName">overtime Approval Type Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// </summary>        
        /// <returns>The OvertimeApprovalTypes List</returns>
        public PageHelper<OvertimeApprovalTypesEntity> GetOvertimeApprovalTypesList(string geographicDivisionCode, int divisionCode, int overtimeApprovalTypeCode, string overtimeApprovalTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<OvertimeApprovalTypesEntity> response;
            try
            {
                response = overtimeApprovalTypesDAL.GetOvertimeApprovalTypesList(
                    geographicDivisionCode,
                    divisionCode,
                    overtimeApprovalTypeCode,
                    overtimeApprovalTypeName,
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
        /// Get Overtime Approval Types By Overtime Approval Type Code
        /// </summary>        
        /// <param name="overtimeApprovalTypeCode">Overtime Approval Type Code</param> 
        /// <returns>OvertimeApprovalTypesEntity</returns>
        public OvertimeApprovalTypesEntity OvertimeApprovalTypesByOvertimeApprovalTypeCode(int overtimeApprovalTypeCode)
        {
            try
            {
                return overtimeApprovalTypesDAL.OvertimeApprovalTypesByOvertimeApprovalTypeCode(overtimeApprovalTypeCode);
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
        /// Update the Overtime Approval Types
        /// </summary>
        /// <param name="overtimeApprovalTypes">Overtime Approval Types</param>
        public bool UpdateOvertimeApprovalTypes(OvertimeApprovalTypesEntity overtimeApprovalTypes)
        {
            try
            {
                return overtimeApprovalTypesDAL.UpdateOvertimeApprovalTypes(overtimeApprovalTypes);
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
