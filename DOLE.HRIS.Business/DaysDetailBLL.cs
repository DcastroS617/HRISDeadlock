using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class DaysDetailBLL : IDaysDetailBLL<DaysDetailEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IDaysDetailDAL<DaysDetailEntity> daysDetailDAL;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="daysDetailDAL">Data access object</param>
        public DaysDetailBLL(IDaysDetailDAL<DaysDetailEntity> daysDetailDAL)
        {
            this.daysDetailDAL = daysDetailDAL;
        }

        /// <summary>
        /// Get list the Days Detail
        /// </summary>    
        /// <param name="daysDetailCode">days Detail Code</param>
        /// <param name="dayTypeCode">day Type Code</param>
        /// <param name="codeDateApplies">code Date Applies</param>
        /// <param name="codeDateBase">code Date Base</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        public PageHelper<DaysDetailEntity> GetDaysDetailList(string geographicDivisionCode, int divisionCode, int daysDetailCode, int dayTypeCode, string descriptionDay, DateTime? codeDateBase, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<DaysDetailEntity> response = new PageHelper<DaysDetailEntity>();
            try
            {
                response = daysDetailDAL.GetDaysDetailList(
                    geographicDivisionCode
                    , divisionCode
                    , daysDetailCode
                    , dayTypeCode
                    , descriptionDay
                    , codeDateBase
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
        /// Get Days Detail By Days Detail Code
        /// </summary>        
        /// <param name="daysDetailCode">Days Detail Code</param> 
        /// <returns>DaysDetailEntity</returns>
        public DaysDetailEntity GetDaysDetailByDaysDetailCode(int daysDetailCode)
        {
            try
            {
                return daysDetailDAL.GetDaysDetailByDaysDetailCode(daysDetailCode);
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
        /// Get Days Detail By Date  Apllies
        /// </summary>        
        /// <param name="daysDetailEntity">Days Detail Entity</param> 
        /// <returns> A List of DaysDetailEntity</returns>
        public List<DaysDetailEntity> GetDaysDetailByDate(DaysDetailEntity daysDetailEntity)
        {
            try
            {
                return daysDetailDAL.GetDaysDetailByDate(daysDetailEntity);
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
        /// Save the Days Detail
        /// </summary>
        /// <param name="daysDetailEntity">days Detail Entity</param> 
        public bool AddDaysDetail(DaysDetailEntity daysDetail)
        {
            try
            {
                return daysDetailDAL.AddDaysDetail(daysDetail);
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
        /// Update the Days Detail
        /// </summary>
        /// <param name="daysDetailEntity">days Detail Entity</param>
        public bool UpdateDaysDetail(DaysDetailEntity daysDetail)
        {
            try
            {
                return daysDetailDAL.UpdateDaysDetail(daysDetail);
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
        /// Delete a Days Detail
        /// </summary>
        /// <param name="daysDetailCode">Days Detail Code</param>
        public bool DeleteDaysDetail(int daysDetailCode)
        {
            try
            {
                return daysDetailDAL.DeleteDaysDetail(daysDetailCode);
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
