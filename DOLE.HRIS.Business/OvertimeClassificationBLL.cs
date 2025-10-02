using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class OvertimeClassificationBLL : IOvertimeClassificationBLL<OvertimeClassificationEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOvertimeClassificationDAL<OvertimeClassificationEntity> overtimeClassificationDAL;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overtimeClassificationDAL">Data access object</param>
        public OvertimeClassificationBLL(IOvertimeClassificationDAL<OvertimeClassificationEntity> overtimeClassificationDAL)
        {
            this.overtimeClassificationDAL = overtimeClassificationDAL;
        }

        /// <summary>
        /// Get list the Overtime Classification
        /// </summary>    
        /// <param name="overtimeClassification">overtime Classification</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The OvertimeClassificationEntity List</returns>
        public PageHelper<OvertimeClassificationEntity> GetOvertimeClassificationList(int daytype, string sortExpression, string sortDirection, int pageNumber, int? pageSize, OvertimeClassificationEntity overtimeClassification)
        {
            PageHelper<OvertimeClassificationEntity> response = new PageHelper<OvertimeClassificationEntity>();
            try
            {
                response = overtimeClassificationDAL.GetOvertimeClassificationList(
                      daytype
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSize
                    , overtimeClassification);
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
        /// Get Overtime Classification By Code
        /// </summary>        
        /// <param name="overtimeClassificationCode">Overtime Classification Code</param> 
        /// <returns>OvertimeClassificationEntity</returns>
        public OvertimeClassificationEntity GetOvertimeClassificationByCode(int overtimeClassificationCode)
        {
            try
            {
                return overtimeClassificationDAL.GetOvertimeClassificationByCode(overtimeClassificationCode);
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
        /// Save the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationEntity">overtime Classification Entity</param> 
        public bool AddOvertimeClassification(string geographicDivisionCode, int divisionCode, OvertimeClassificationEntity overtimeClassification)
        {
            try
            {
                return overtimeClassificationDAL.AddOvertimeClassification(geographicDivisionCode, divisionCode, overtimeClassification);
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
        /// Update the Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationEntity">overtime Classification Entity</param>
        public bool UpdateOvertimeClassification(OvertimeClassificationEntity overtimeClassification)
        {
            try
            {
                return overtimeClassificationDAL.UpdateOvertimeClassification(overtimeClassification);
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
        /// Delete a Overtime Classification
        /// </summary>
        /// <param name="overtimeClassificationCode">Overtime Classification Code</param>
        public bool DeleteOvertimeClassification(int overtimeClassificationCode)
        {
            try
            {
                return overtimeClassificationDAL.DeleteOvertimeClassification(overtimeClassificationCode);
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
        /// Get list the Overtime Classifications
        /// </summary>
        /// <param name="geographicDivisionCode">geographic Division Code</param> 
        /// <param name="geographicDivisionCode">division Code</param> 
        /// <returns>The OvertimeClassificationEntity list</returns>
        public List<OvertimeClassificationEntity> GetOvertimeClassificationsList(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                return overtimeClassificationDAL.GetOvertimeClassificationsList(geographicDivisionCode, divisionCode);
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
