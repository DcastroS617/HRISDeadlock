using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class DayTypesBLL : IDayTypesBLL<DayTypesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IDayTypesDAL<DayTypesEntity> dayTypesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="dayTypesDAL">Data access object</param>
        public DayTypesBLL(IDayTypesDAL<DayTypesEntity> dayTypesDAL)
        {
            this.dayTypesDAL = dayTypesDAL;
        }

        /// <summary>
        /// Get list the Day Types
        /// </summary>   
        /// <param name="dayTypeCode">day Type Code</param>
        /// <param name="dayTypesName">day Types Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The DayTypesEntity List</returns>
        public PageHelper<DayTypesEntity> GetDayTypesList(string geographicDivisionCode, int divisionCode, int dayTypeCode, string dayTypesName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<DayTypesEntity> response = new PageHelper<DayTypesEntity>();
            try
            {
                response = dayTypesDAL.GetDayTypesList(
                      geographicDivisionCode 
                    , divisionCode
                    , dayTypeCode
                    , dayTypesName
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
        /// Get Day Types By Day Type Code
        /// </summary>        
        /// <param name="dayTypeCode">Day Type Code</param> 
        /// <returns>DayTypesEntity</returns> 
        public DayTypesEntity GetDayTypesByDayTypeCode(int dayTypeCode)
        {
            try
            {
                return dayTypesDAL.GetDayTypesByDayTypeCode(dayTypeCode);
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
        /// Save the DayTypes
        /// </summary>
        /// <param name="dayTypesEntity">day Types Entity</param>
        public bool AddDayTypes(DayTypesEntity dayTypes)
        {
            try
            {
                return dayTypesDAL.AddDayTypes(dayTypes);
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
        /// Update the Day Type
        /// </summary>
        /// <param name="dayTypesEntity">day Types Entity</param> 
        public bool UpdateDayType(DayTypesEntity dayTypes)
        {
            try
            {
                return dayTypesDAL.UpdateDayType(dayTypes);
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
        /// Delete a Day Type
        /// </summary>
        /// <param name="dayTypeCode">Day Type Code</param>
        public bool DeleteDayType(int dayTypeCode)
        {
            try
            {
                return dayTypesDAL.DeleteDayType(dayTypeCode);
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
        /// Get list Day Types For Dropdown
        /// </summary>        
        /// <returns>The DayTypesEntity list</returns>
        public List<DayTypesEntity> GetDayTypesListForDropdown(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                return dayTypesDAL.GetDayTypesListForDropdown(geographicDivisionCode,  divisionCode);
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
