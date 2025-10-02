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
    public class OvertimeTypesBLL : IOvertimeTypesBLL<OvertimeTypesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOvertimeTypesDAL<OvertimeTypesEntity> overtimeTypesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overtimeTypesDAL">Data access object</param>
        public OvertimeTypesBLL(IOvertimeTypesDAL<OvertimeTypesEntity> overtimeTypesDAL)
        {
            this.overtimeTypesDAL = overtimeTypesDAL;
        }

        /// <summary>
        /// Get List the Overtime Types
        /// </summary>        
        /// <param name="overtimeTypeCode">overtime Type Code</param>
        /// <param name="overtimeTypeName">overtime Type Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The OvertimeTypesEntity List</return>
        public PageHelper<OvertimeTypesEntity> GetOvertimeTypesList(string geographicDivisionCode, int divisionCode, int overtimeTypeCode, string overtimeTypeName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<OvertimeTypesEntity> response = new PageHelper<OvertimeTypesEntity>();
            try
            {
                response = overtimeTypesDAL.GetOvertimeTypesList(
                    geographicDivisionCode
                    , divisionCode
                    , overtimeTypeCode
                    , overtimeTypeName
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
        /// Get Overtime Types By Overtime Type Code
        /// </summary>        
        /// <param name="overtimeTypeCode">Overtime Type Code</param> 
        /// <return>Overtime Types Entity</return>
        public OvertimeTypesEntity GetOvertimeTypesByOvertimeTypeCode(int overtimeTypeCode)
        {
            try
            {
                return overtimeTypesDAL.GetOvertimeTypesByOvertimeTypeCode(overtimeTypeCode);
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
        /// Save the Overtime Types
        /// </summary>
        /// <param name="overtimeTypesEntity">overtime Types Entity</param>
        public bool AddOvertimeTypes(OvertimeTypesEntity overtimeTypes)
        {
            try
            {
                return overtimeTypesDAL.AddOvertimeTypes(overtimeTypes);
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
        /// Update the Overtime Types
        /// </summary>
        /// <param name="overtimeTypesEntity">overtime Types Entity</param> 
        public bool UpdateOvertimeTypes(OvertimeTypesEntity overtimeTypes)
        {
            try
            {
                return overtimeTypesDAL.UpdateOvertimeTypes(overtimeTypes);
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
        /// Delete Overtime Types
        /// </summary>
        /// <param name="overtimeTypeCode">Overtime Type Code</param>
        public bool DeleteOvertimeTypes(int overtimeTypeCode)
        {
            try
            {
                return overtimeTypesDAL.DeleteOvertimeTypes(overtimeTypeCode);
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
        /// Get Overtime Types List For Dropdown
        /// </summary>
        /// <return>The OvertimeTypesEntity List</return>
        public List<OvertimeTypesEntity> GetOvertimeTypesListForDropdown()
        {
            try
            {
                return overtimeTypesDAL.GetOvertimeTypesListForDropdown();
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
